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
        #region PROPERTIES

        public string Base { get; private set; }

        #endregion



        #region CONSTRUCTORS

        public ApiConfiguration(IConfiguration configuration)
        {
            Base = configuration.GetSection("API")["Base"];
        }

        #endregion
    }
}
