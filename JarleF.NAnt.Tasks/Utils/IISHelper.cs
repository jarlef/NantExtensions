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
                                       Status = GetSiteStatus(s)
                                   };

            return query.ToList();
        }

        public static IISSite GetSite(string computerName, string siteName)
        {
            return GetSites(computerName).Where(s => string.Compare(s.Name, siteName, true) == 0).SingleOrDefault();
        }

        public static void Start(string computerName, long siteId)
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

        public static void Stop(string computerName, long siteId)
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

        private static IISSiteStatus GetSiteStatus(Site site)
        {
            switch (site.State)
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
