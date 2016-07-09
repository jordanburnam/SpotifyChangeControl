using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web;
using SpotifyChangeControlLib;
using SpotifyChangeControlLib.DataObjects;
using SpotifyAPI.Web.Models;

namespace SpotifyChangeControlCMD
{
    class Program
    {
        private static string SCC_PRIVATE_ID;
        private static string SCC_PUBLIC_ID;
        private static string SCC_SQL_CON;
        private static string SCC_REDIS_HOST;
        private static string SCC_REDIS_PASS;
        private static int SCC_REDIS_PORT;
        static void Main(string[] args)
        {
            Console.WriteLine("Started @ " + DateTime.Now);
            if (args.Length == 0)
            {
                SCC_PRIVATE_ID = System.Environment.GetEnvironmentVariable("SCC_PRIVATE_ID");
                SCC_PUBLIC_ID = System.Environment.GetEnvironmentVariable("SCC_PUBLIC_ID");
              
                SCC_SQL_CON = System.Environment.GetEnvironmentVariable("SCC_SQL_CON");

                SCC_REDIS_HOST = System.Environment.GetEnvironmentVariable("SCC_REDIS_HOST");
                SCC_REDIS_PASS = System.Environment.GetEnvironmentVariable("SCC_REDIS_PASS");
                string sPort = System.Environment.GetEnvironmentVariable("SCC_REDIS_PORT");
                if (!int.TryParse(sPort, out SCC_REDIS_PORT))
                {
                    SCC_REDIS_PORT = 6379;
                }
            }
            else
            {
                SCC_PRIVATE_ID = GetParameterValueByNameFromArgs("PRV", args);
                SCC_PUBLIC_ID = GetParameterValueByNameFromArgs("PUB", args); ;

                SCC_SQL_CON = GetParameterValueByNameFromArgs("SSC", args); ;

                SCC_REDIS_HOST = GetParameterValueByNameFromArgs("SRH", args); ;
                SCC_REDIS_PASS = GetParameterValueByNameFromArgs("SRP", args); ;
                string sPort = GetParameterValueByNameFromArgs("P", args); ;
                if (!int.TryParse(sPort, out SCC_REDIS_PORT))
                {
                    SCC_REDIS_PORT = 6379;
                }
            }
            SCCManager oSCCManager = new SCCManager();
            
            oSCCManager.ExecuteStateMananger();
            Console.WriteLine("DONE at " + DateTime.Now);
            Console.ReadLine();
            //TODO:Do things with stuff
           

            
        }

        static string GetParameterValueByNameFromArgs(string sName, string[] args)
        {
            string sValue = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].ToUpper().Equals(sName.ToUpper()))
                {
                    if (i + 1 < args.Length)
                    {
                        sValue = args[i + 1];
                    }
                }
            }
            return sValue;
        }

   

        
    }
}
