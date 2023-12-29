using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.Website.Services.Website.AuthBackground
{
    public class WebsiteAuthBackgroundConfiguration : WebsiteConfiguration
    {
        #region PROPERTIES

        public string WebsiteAuthBackgroundBase { get; private set; }
        public string WebsiteAuthBackgroundGetRandom { get; private set; }
        public string WebsiteAuthBackgroundGet { get; private set; }
        public string WebsiteAuthBackgroundGetAll { get; private set; }
        public string WebsiteAuthBackgroundAdd { get; private set; }
        public string WebsiteAuthBackgroundModify { get; private set; }
        public string WebsiteAuthBackgroundDelete { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public WebsiteAuthBackgroundConfiguration(IConfiguration configuration) : base(configuration)
        {
            WebsiteAuthBackgroundBase = $"{WebsiteBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["Base"]}";
            WebsiteAuthBackgroundGetRandom = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["GetRandom"]}";
            WebsiteAuthBackgroundGet = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["Get"]}";
            WebsiteAuthBackgroundGetAll = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["GetAll"]}";
            WebsiteAuthBackgroundAdd = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["Add"]}";
            WebsiteAuthBackgroundModify = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["Modify"]}";
            WebsiteAuthBackgroundModify = $"{WebsiteAuthBackgroundBase}{configuration.GetSection("API").GetSection("Website").GetSection("AuthBackground")["Delete"]}";
        }

        #endregion
    }
}
