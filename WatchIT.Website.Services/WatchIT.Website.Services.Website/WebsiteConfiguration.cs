using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.Website.Services.Website
{
    public class WebsiteConfiguration : ApiConfiguration
    {
        #region PROPERTIES

        public string WebsiteBase { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public WebsiteConfiguration(IConfiguration configuration) : base(configuration)
        {
            WebsiteBase = $"{Base}{configuration.GetSection("API").GetSection("Website")["Base"]}";
        }

        #endregion
    }
}
