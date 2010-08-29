using JarleF.NAnt.Tasks.Utils;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace JarleF.NAnt.Tasks
{
    [TaskName("iissitestatus")]
    public class IISSiteStatusTask : Task
    {
        public IISSiteStatusTask()
        {
            Machine = "localhost";
        }

        [TaskAttribute("machine")]
        [StringValidator(AllowEmpty = false)]
        public string Machine { get; set; }

        [TaskAttribute("sitename", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string SiteName { get; set; }

        [TaskAttribute("propertyname", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string PropertyName { get; set; }

        protected override void ExecuteTask()
        {
            var site = IISHelper.GetSite(Machine, SiteName);
            
            if (site == null)
            {
                throw new BuildException("Could not find site '" + SiteName + "' on '" + Machine + "'");
            }

            if (Project.Properties.Contains(PropertyName) == true)
            {
                Project.Properties[PropertyName] = site.Status.ToString();
            }
            else
            {
                Project.Properties.Add(PropertyName, site.Status.ToString());
            }
        }
    }
}
