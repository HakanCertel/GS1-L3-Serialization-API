using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GS1L3.Persistence
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager configurationManager = new ConfigurationManager();
                configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/GS1L3.API"));
                configurationManager.AddJsonFile("appsettings.json");

                return configurationManager.GetConnectionString("DefaultConnection");
            }
        }
    }
}
