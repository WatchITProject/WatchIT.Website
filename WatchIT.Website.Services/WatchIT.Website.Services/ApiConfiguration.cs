using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.Website.Services
{
    public class ApiConfiguration
    {
        #region FIELDS

        protected readonly IConfiguration _configuration;

        #endregion



        #region PROPERTIES

        public string Base { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public ApiConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;

            Base = _configuration.GetSection("API")["Base"];
        }

        #endregion
    }
}
