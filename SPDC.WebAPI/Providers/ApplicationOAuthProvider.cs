using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using SPDC.Common;
using SPDC.Data;
using SPDC.Model.Models;
using SPDC.WebAPI.Models;

namespace SPDC.WebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        //public ApplicationOAuthProvider(string publicClientId)
        //{
        //    if (publicClientId == null)
        //    {
        //        throw new ArgumentNullException("publicClientId");
        //    }

        //    _publicClientId = publicClientId;
        //}

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var langCode = context.Request.Headers.Get("Accept-Language");
            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            var dbContext = new ApplicationDbContext();
            ApplicationUser user = null;

            var isAdmin = HttpContext.Current.Request.Params["m"] != null && HttpContext.Current.Request.Params["m"] == "1";
            //HttpContext.Current.Request.
            if (isAdmin)
            {
                var ldapResult = LDAPAuthenticated(context.UserName, context.Password);
                if (ldapResult != null)
                {
                    user = await userManager.FindByNameAsync(context.UserName);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "Invalid User");
                        return;
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Invalid Username or Password");
                    return;
                }
            }
            else
            {

                if (RegexUtilities.IsValidEmail(context.UserName))
                {
                    //user = await userManager.FindByEmailAsync(context.UserName);
                    user = dbContext.Users.Where(u => u.Email.Equals(context.UserName)).SingleOrDefault();
                }
                else
                {
                    user = dbContext.Users.Where(u => u.CICNumber == context.UserName).SingleOrDefault();
                }

                if (user == null)
                {
                    if (user == null)
                    {
                        context.SetError("invalid_grant", GetLoginErrorMessage("Login", "invalid_login_message_label", langCode));
                        return;
                    }
                }

                user = await userManager.FindAsync(user.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", GetLoginErrorMessage("Login", "invalid_login_message_label", langCode));
                    return;
                }

                if (!user.EmailConfirmed)
                {
                    context.SetError("invalid_grant", GetLoginErrorMessage("Login", "account_not_comfirmed_message_label", langCode));
                    return;
                }

                var form = await context.Request.ReadFormAsync();
                var deviceId = form["device_id"];

                if (!string.IsNullOrWhiteSpace(deviceId))
                {
                    var device = dbContext.UserDevices.SingleOrDefault(x => x.DeviceToken.Equals(deviceId));
                    if (device == null)
                    {
                        UserDevice userDevice = new UserDevice() { DeviceToken = deviceId, UserId = user.Id };
                        dbContext.UserDevices.Add(userDevice);
                    }
                    else
                    {
                        device.UserId = user.Id;
                        dbContext.UserDevices.Attach(device);
                        dbContext.Entry(device).State = EntityState.Modified;
                    }

                    dbContext.SaveChanges();
                }
            }


            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager);
            if (isAdmin)
            {
                oAuthIdentity.AddClaim(new Claim("ldapUsername", context.UserName));
                oAuthIdentity.AddClaim(new Claim("ldapPassword", context.Password));
            }

            ClaimsIdentity cookiesIdentity = await user.GenerateUserIdentityAsync(userManager);

            AuthenticationProperties properties = CreateProperties(user);
            AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            // Resource owner password credentials does not provide a client ID.
            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }
            //if (context.ClientId != null)
            //{
            //    context.Validated();
            //}

            context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == _publicClientId)
            {
                System.Uri expectedRootUri = new System.Uri(context.Request.Uri, "/");

                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"IsAdmin", (user.Roles.Where(r => r.RoleId == 2).ToArray().Length > 0).ToString() },
                {"IsSuperAdmin", (user.Roles.Where(r => r.RoleId == 1).ToArray().Length > 0).ToString() },
                {"Email",user.Email == null ? string.Empty : user.Email},
                {"CICNumber",user.CICNumber == null ? string.Empty : user.CICNumber },
                {"SurnameEN", user.Particular == null ? string.Empty : user.Particular.SurnameEN },
                {"GivenNameEN", user.Particular == null ? string.Empty : user.Particular.GivenNameEN },
                {"SurnameCN", user.Particular == null ? string.Empty : user.Particular.SurnameCN },
                {"GivenNameCN", user.Particular == null ? string.Empty : user.Particular.GivenNameCN },
                {"Username", user.UserName},
            };
            return new AuthenticationProperties(data);
        }

        public static SearchResult LDAPAuthenticated(string login, string password)
        {
            try
            {
                DirectoryEntry ldap = new DirectoryEntry(SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.LDAPUrl), SystemParameterProvider.Instance.GetValueString(SystemParameterInfo.LDAPAccountPrefix) + @"\" + login, password);
                DirectorySearcher searcher = new DirectorySearcher(ldap);
                searcher.Filter = "(SAMAccountName=" + login + ")";
                SearchResult result = searcher.FindOne();
                return result;
            }
            catch (DirectoryServicesCOMException ex)
            {
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetLoginErrorMessage(string formName, string errorname, string langCode)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath($"~/MultiLanguage/{formName + langCode}.json");
            try
            {
                JObject o1 = JObject.Parse(File.ReadAllText(path));
                return o1.Property(errorname).Value.ToString();
            }
            catch
            {
                return "File multi language is not found";
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
    }
}