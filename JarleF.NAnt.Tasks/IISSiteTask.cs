using JarleF.NAnt.Tasks.Core;
using JarleF.NAnt.Tasks.Utils;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace JarleF.NAnt.Tasks
{
    [TaskName("iissite")]
    public class IISSiteTask : Task
    {
        public IISSiteTask()
        {
            Machine = "localhost";
        }

        [TaskAttribute("machine")]
        [StringValidator(AllowEmpty = false)]
        public string Machine { get; set; }

        [TaskAttribute("sitename", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string SiteName { get; set; }

        [TaskAttribute("action", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Action { get; set; }

        protected override void ExecuteTask()
        {
            var site = IISHelper.GetSite(Machine, SiteName);
            var action = Action.ToLower();

            if (site == null)
            {
                throw new BuildException("Could not find site '{0}' on '{1}'".FormatString(SiteName, Machine));
            }

            switch (action)
            {
                case "start":
                    IISHelper.Start(Machine, site.Id);
                    break;
                case "stop":
                    IISHelper.Stop(Machine, site.Id);
                    break;
                default:
                    throw new BuildException("Invalid action provided");
            }
           
            site = IISHelper.GetSite(Machine, site.Name);

            if (action == "stop" && site.Status != IISSiteStatus.Stopped && site.Status != IISSiteStatus.Unkown)
            {
                throw new BuildException("Failed to stop site '{0}' on '{1}'".FormatString(site.Name, Machine));
            }

            if (action == "start" && site.Status != IISSiteStatus.Started && site.Status != IISSiteStatus.Unkown)
            {
                throw new BuildException("Failed to start site '{0}' on '{1}'".FormatString(site.Name, Machine));
            }

            Project.Log(Level.Info, "{0} site '{1}' on '{2}'".FormatString(site.Status, site.Name, Machine));
        }
    }
}
