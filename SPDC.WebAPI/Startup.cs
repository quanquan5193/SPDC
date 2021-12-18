using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SPDC.Data;
using SPDC.Data.Infrastructure;
using SPDC.Data.Repositories;
using SPDC.Service.Services;
using Hangfire;
using Hangfire.SqlServer;
using SPDC.WebAPI.Helpers;
using SPDC.Common;
using System.Configuration;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using SPDC.Service;

[assembly: OwinStartup(typeof(SPDC.WebAPI.Startup))]

namespace SPDC.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SystemParameterProvider.Initialize(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            ConfigureAuth(app);
            ConfigAutofac(app);
            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            RecurringJob.AddOrUpdate(() => FireBackgroundTaskHelper.CheckInvoiceOverdue(), Cron.Daily);
            RecurringJob.AddOrUpdate(() => FireBackgroundTaskHelper.AutoUpdateCMSStatus(), Cron.Daily);
            RecurringJob.AddOrUpdate(() => FireBackgroundTaskHelper.AutoUpdateEnrollmentStatus(), Cron.Daily);
            RecurringJob.AddOrUpdate(() => FireBackgroundTaskHelper.AutoEmail().RunSynchronously(), Cron.Daily);

        }

        private void ConfigAutofac(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Infrastructure
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<ApplicationDbContext>().As<ApplicationDbContext>().InstancePerRequest();

            // Identity
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).InstancePerRequest();
            builder.Register(c => app.GetDataProtectionProvider()).InstancePerRequest();

            // Repositories
            builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces().InstancePerRequest();

            // Services
            builder.RegisterAssemblyTypes(typeof(UserService).Assembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces().InstancePerRequest();

            Autofac.IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);

        }

        private IEnumerable<IDisposable> GetHangfireServers()
        {
            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
    }
}
