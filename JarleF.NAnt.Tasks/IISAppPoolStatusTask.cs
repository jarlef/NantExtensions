using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarleF.NAnt.Tasks.Utils;
using NAnt.Core;
using NAnt.Core.Attributes;

namespace JarleF.NAnt.Tasks
{
    [TaskName("iisapppool2status")]
    public class IISAppPoolStatusTask : Task
    {
        public IISAppPoolStatusTask()
        {
            Machine = "localhost";
        }

        [TaskAttribute("machine")]
        [StringValidator(AllowEmpty = false)]
        public string Machine { get; set; }

        [TaskAttribute("apppoolname", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string AppPoolName { get; set; }

        [TaskAttribute("propertyname", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string PropertyName { get; set; }

        protected override void ExecuteTask()
        {
            var applicationPool = IISHelper.GetApplicationPool(Machine, AppPoolName);

            if (applicationPool == null)
            {
                throw new BuildException("Could not find application pool '{0}' on '{1}'".FormatString(AppPoolName, Machine));
            }

            if (Project.Properties.Contains(PropertyName) == true)
            {
                Project.Properties[PropertyName] = applicationPool.Status.ToString();
            }
            else
            {
                Project.Properties.Add(PropertyName, applicationPool.Status.ToString());
            }
        }
    }
}
