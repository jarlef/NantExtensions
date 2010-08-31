using System;
using System.Collections.Generic;
using System.Linq;
using JarleF.NAnt.Tasks.Core;
using Microsoft.Web.Administration;

namespace JarleF.NAnt.Tasks.Utils
{
    public static class IISHelper
    {
        public static List<IISSite> GetSites(string computerName)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);

            var query = from s in iisManager.Sites
                        select new IISSite()
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       Status = GetStatus(s)
                                   };

            return query.ToList();
        }

        public static List<IISAppPool> GetApplicationPools(string computerName)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);

            var query = from a in iisManager.ApplicationPools.OfType<ApplicationPool>()
                        select new IISAppPool()
                                   {
                                       Name = a.Name,
                                       Status = GetStatus(a)
                                   };
            
            return query.ToList();
        }

        public static IISSite GetSite(string computerName, string siteName)
        {
            return GetSites(computerName).Where(s => string.Compare(s.Name, siteName, true) == 0).SingleOrDefault();
        }

        public static IISAppPool GetApplicationPool(string computerName, string appPoolName)
        {
            return GetApplicationPools(computerName).Where(a => string.Compare(a.Name, appPoolName, true) == 0).SingleOrDefault();
        }

        public static void StartSite(string computerName, long siteId)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);
            
            var query = from s in iisManager.Sites
                         where s.Id == siteId
                         select s;

            var site = query.SingleOrDefault();

            if (site != null)
            {
                site.Start();
            }
        }

        public static void StopSite(string computerName, long siteId)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);
            
            var query = from s in iisManager.Sites
                        where s.Id == siteId
                        select s;

            var site = query.SingleOrDefault();

            if (site != null)
            {
                site.Stop();
            }
        }

        public static void StartApplicationPool(string computerName, string appPoolName)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);

            var applicationPool = iisManager.ApplicationPools[appPoolName];
            
            if(applicationPool == null)
            {
                return;
            }

            var state = GetStatus(applicationPool);

            if (state != IISSiteStatus.Started && state != IISSiteStatus.Starting)
            {
                applicationPool.Start();
            }
        }

        public static void StopApplicationPool(string computerName, string appPoolName)
        {
            ServerManager iisManager = ServerManager.OpenRemote(computerName);
            
            var applicationPool = iisManager.ApplicationPools[appPoolName];

            if (applicationPool == null)
            {
                return;
            }

            var state = GetStatus(applicationPool);

            if (state != IISSiteStatus.Stopped && state != IISSiteStatus.Stopping)
            {
                applicationPool.Stop();
            }
        }


        private static IISSiteStatus GetStatus(ApplicationPool applicationPool)
        {
            try
            {
                return GetStatus(applicationPool.State);
            }
            catch(Exception)
            {
                return IISSiteStatus.Unkown;
            }
        }

        private static IISSiteStatus GetStatus(Site site)
        {
            try
            {
                return GetStatus(site.State);
            }
            catch (Exception)
            {
                return IISSiteStatus.Unkown;
            }
        }

        private static IISSiteStatus GetStatus(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Starting:
                    return IISSiteStatus.Starting;
                case ObjectState.Started:
                    return IISSiteStatus.Started;
                case ObjectState.Stopped:
                    return IISSiteStatus.Stopped;
                case ObjectState.Stopping:
                    return IISSiteStatus.Stopping;
                default:
                    return IISSiteStatus.Unkown;
            }
        }
        

    }
}
