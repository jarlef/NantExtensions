using System;
using JarleF.NAnt.Tasks.Utils;

namespace JarleF.Nant.Tasks.ManualTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                const string machineName = "localhost";
                const string siteName = "Default Web Site";
                const string appPoolName = "DefaultAppPool";

                var sites = IISHelper.GetSites(machineName);

                foreach (var site in sites)
                {
                    Console.WriteLine(site.ToString());
                }

                var defaultWebSite = IISHelper.GetSite(machineName, siteName);

                Console.WriteLine(defaultWebSite);

                IISHelper.StopSite(machineName, defaultWebSite.Id);
                defaultWebSite = IISHelper.GetSite(machineName, siteName);
                Console.WriteLine(defaultWebSite);

                IISHelper.StartSite(machineName, defaultWebSite.Id);
                defaultWebSite = IISHelper.GetSite(machineName, siteName);
                Console.WriteLine(defaultWebSite);

                var defaultAppPool = IISHelper.GetApplicationPool(machineName, appPoolName);

                Console.WriteLine(defaultAppPool);

                IISHelper.StopApplicationPool(machineName, appPoolName);
                defaultAppPool = IISHelper.GetApplicationPool(machineName, appPoolName);
                Console.WriteLine(defaultAppPool);

                IISHelper.StartApplicationPool(machineName, appPoolName);
                defaultAppPool = IISHelper.GetApplicationPool(machineName, appPoolName);
                Console.WriteLine(defaultAppPool);

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
