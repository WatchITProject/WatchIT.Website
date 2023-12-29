using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.Website.Services.Accounts
{
    public class AccountsConfiguration : ApiConfiguration
    {
        #region PROPERTIES

        public string AccountsBase { get; private set; }
        public string AccountsRegister { get; private set; }
        public string AccountsAuthenticate { get; private set; }
        public string AccountsAuthenticateRefresh { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public AccountsConfiguration(IConfiguration configuration) : base(configuration)
        {
            AccountsBase = $"{Base}{configuration.GetSection("API").GetSection("Accounts")["Base"]}";
            AccountsAuthenticate = $"{AccountsBase}{configuration.GetSection("API").GetSection("Accounts")["Authenticate"]}";
            AccountsAuthenticateRefresh = $"{AccountsBase}{configuration.GetSection("API").GetSection("Accounts")["AuthenticateRefresh"]}";
        }

        #endregion
    }
}
