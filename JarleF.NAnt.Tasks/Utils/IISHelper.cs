using System;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using JarleF.NAnt.Tasks.Core;

namespace JarleF.NAnt.Tasks.Utils
{
    public static class IISHelper
    {
        public static List<IISSite> GetSites(string computerName)
        {
            ManagementObjectCollection sites = CreateClassObject(computerName, "Site").GetInstances();

            var query = from s in sites.OfType<ManagementObject>()
                              select new IISSite()
                              {
                                  Id = Convert.ToInt32(s.GetPropertyValue("Id")),
                                  Name = (string) s.GetPropertyValue("Name"),
                                  Status = GetSiteStatus(s)
                              };

            return query.ToList();
        }

        public static IISSite GetSite(string computerName, string siteName)
        {
            return GetSites(computerName).Where(s => string.Compare(s.Name, siteName, true) == 0).SingleOrDefault();
        }

        public static void Start(string computerName, int siteId)
        {
            ManagementObjectCollection sites = CreateClassObject(computerName, "Site").GetInstances();

            var query = from s in sites.OfType<ManagementObject>()
                        where Convert.ToInt32(s.GetPropertyValue("Id")) == siteId
                        select s;

            var site = query.SingleOrDefault();

            if (site != null)
            {
                site.InvokeMethod("Start", new object[0]);
            }
        }

        public static void Stop(string computerName, int siteId)
        {
           ManagementObjectCollection sites = CreateClassObject(computerName, "Site").GetInstances();

            var query = from s in sites.OfType<ManagementObject>()
                        where Convert.ToInt32(s.GetPropertyValue("Id")) == siteId
                        select s;

            var site = query.SingleOrDefault();

            if(site != null)
            {
                site.InvokeMethod("Stop", new object[0]);
            }
        }

        private static ManagementClass CreateClassObject(string computerName, string path)
        {
            var connection = new ConnectionOptions
            {
                Impersonation = ImpersonationLevel.Impersonate,
                Authentication = AuthenticationLevel.PacketPrivacy
            };

            var scope = new ManagementScope("\\\\" + computerName + "\\root\\webadministration", connection);
           
            return new ManagementClass(
                scope,
                new ManagementPath(path),
                new ObjectGetOptions()
             );
        }

        private static IISSiteStatus GetSiteStatus(ManagementObject site)
        {
            var status = (uint) site.InvokeMethod("GetState", new object[0]);
            switch (status)
            {
                case 0:
                    return IISSiteStatus.Starting;
                case 1:
                    return IISSiteStatus.Started;
                case 3:
                    return IISSiteStatus.Stopped;
                default:
                    return IISSiteStatus.Unkown;
            }
        }

    }
}
