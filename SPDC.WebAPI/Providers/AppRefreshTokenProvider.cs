using Microsoft.Owin.Security.Infrastructure;
using SPDC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPDC.WebAPI.Providers
{
    public class AppRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext _context)
        {
            _context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddMinutes(SystemParameterProvider.Instance.GetValueInt(SystemParameterInfo.SessionTimeout) * 2 )); //  Extending 5 minutes before expiration of token.  
            _context.SetToken(_context.SerializeTicket());
        }
        public override void Receive(AuthenticationTokenReceiveContext _context)
        {
            _context.DeserializeTicket(_context.Token);
        }
    }
}