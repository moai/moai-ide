using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public class JenkinsAttribute : BuildSourceAttribute
    {
        public override string Source { get { return this.BuildTag; } }
        public uint BuildNumber { get; private set; }
        public string BuildID { get; private set; }
        public string BuildTag { get; private set; }
        public string BuildUrl { get; private set; }
        public string JobName { get; private set; }
        public string JobUrl { get; private set; }

        public JenkinsAttribute(uint buildNumber, string buildId, string buildTag, string buildUrl, string jobName, string jobUrl)
        {
            this.BuildNumber = buildNumber;
            this.BuildID = buildId;
            this.BuildTag = buildTag;
            this.BuildUrl = buildUrl;
            this.JobName = jobName;
            this.JobUrl = jobUrl;
        }
    }
}
