using JarleF.NAnt.Tasks.Core;
using JarleF.NAnt.Tasks.Utils;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace JarleF.NAnt.Tasks
{
    [TaskName("iisapppool2")]
    public class IISAppPoolTask : Task
    {
        public IISAppPoolTask()
        {
            Machine = "localhost";
        }

        [TaskAttribute("machine")]
        [StringValidator(AllowEmpty = false)]
        public string Machine { get; set; }

        [TaskAttribute("apppoolname", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string AppPoolName { get; set; }

        [TaskAttribute("action", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Action { get; set; }

        protected override void ExecuteTask()
        {
            var applicationPool = IISHelper.GetApplicationPool(Machine, AppPoolName);
            var action = Action.ToLower();

            if (applicationPool == null)
            {
                throw new BuildException("Could not find application pool '{0}' on '{1}'".FormatString(AppPoolName, Machine));
            }

            switch (action)
            {
                case "start":
                    IISHelper.StartApplicationPool(Machine, applicationPool.Name);
                    break;
                case "stop":
                    IISHelper.StopApplicationPool(Machine, applicationPool.Name);
                    break;
                default:
                    throw new BuildException("Invalid action provided");
            }

            applicationPool = IISHelper.GetApplicationPool(Machine, AppPoolName);

            if (action == "stop" && applicationPool.Status != IISSiteStatus.Stopped && applicationPool.Status != IISSiteStatus.Stopping && applicationPool.Status != IISSiteStatus.Unkown)
            {
                throw new BuildException("Failed to stop application pool '{0}' on '{1}'".FormatString(applicationPool.Name, Machine));
            }

            if (action == "start" && applicationPool.Status != IISSiteStatus.Started && applicationPool.Status != IISSiteStatus.Starting && applicationPool.Status != IISSiteStatus.Unkown)
            {
                throw new BuildException("Failed to start application pool '{0}' on '{1}'".FormatString(applicationPool.Name, Machine));
            }

            Project.Log(Level.Info, "{0} application pool '{1}' on '{2}'".FormatString(applicationPool.Status, applicationPool.Name, Machine));
        }
    }
}
