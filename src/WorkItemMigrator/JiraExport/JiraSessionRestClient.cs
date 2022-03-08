using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atlassian.Jira;
using Atlassian.Jira.Remote;

namespace JiraExport
{
    class JiraSessionRestClient : JiraRestClient
    {
        public JiraSessionRestClient(string url, string jsessionid, JiraRestClientSettings settings = null)
            : base(url, new JiraSessionAuthenticator(jsessionid), settings) { }
    }
}
