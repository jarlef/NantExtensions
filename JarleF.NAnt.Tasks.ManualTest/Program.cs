using System;
using JarleF.NAnt.Tasks.Utils;

namespace JarleF.Nant.Tasks.ManualTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var sites = IISHelper.GetSites("localhost");

            foreach (var site in sites)
            {
                Console.WriteLine(site.ToString());
            }

            var defaultWebSite = IISHelper.GetSite("localhost", "Default Web Site");

            Console.WriteLine(defaultWebSite);

            IISHelper.Stop("localhost", defaultWebSite.Id);
            defaultWebSite = IISHelper.GetSite("localhost", defaultWebSite.Name);
            Console.WriteLine(defaultWebSite);

            IISHelper.Start("localhost", defaultWebSite.Id);
            defaultWebSite = IISHelper.GetSite("localhost", defaultWebSite.Name);
            Console.WriteLine(defaultWebSite);

            Console.ReadLine();
        }
    }
}
