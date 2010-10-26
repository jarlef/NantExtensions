using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JarleF.NAnt.Tasks.Utils;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Util;

namespace JarleF.NAnt.Tasks
{
    [TaskName("iniread2")]
    public class IniReadTask : Task
    {
        // Fields
        private string _iniFile;
        private string _key;
        private string _property;
        private string _section;

        public IniReadTask()
        {
            Default = null;
        }

        // Methods
        protected override void ExecuteTask()
        {
            string str = string.Format("Retrieving {0}/{1} from file: {2}", Section, Key, FileName);
            Log(Level.Verbose, LogPrefix + str);
            try
            {
                Properties[Property] = new IniFile(FileName).GetString(Section, Key, Default);
            }
            catch (Exception exception)
            {
                throw new BuildException("Failed: " + str, Location, exception);
            }
        }

        // Properties
        [StringValidator(AllowEmpty = true), TaskAttribute("default", Required = true)]
        public string Default { get; set; }

        [StringValidator(AllowEmpty = false), TaskAttribute("filename", Required = true)]
        public string FileName
        {
            get
            {
                return Project.GetFullPath(_iniFile);
            }
            set
            {
                _iniFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("key", Required = true), StringValidator(AllowEmpty = false)]
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [StringValidator(AllowEmpty = false), TaskAttribute("property", Required = true)]
        public string Property
        {
            get
            {
                return _property;
            }
            set
            {
                _property = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("section", Required = true), StringValidator(AllowEmpty = false)]
        public string Section
        {
            get
            {
                return _section;
            }
            set
            {
                _section = StringUtils.ConvertEmptyToNull(value);
            }
        }
    }


}
