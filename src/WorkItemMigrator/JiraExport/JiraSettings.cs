
namespace JiraExport
{
    public class JiraSettings
    {
        public string Session { get; private set; }
        public string Url { get; private set; }
        public string Project { get; internal set; }
        public string EpicLinkField { get; internal set; }
        public string SprintField { get; internal set; }
        public string UserMappingFile { get; internal set; }
        public int BatchSize { get; internal set; }
        public string AttachmentsDir { get; internal set; }
        public string JQL { get; internal set; }
        public bool UsingJiraCloud { get; internal set; }

        public JiraSettings(string session, string url, string project)
        {
            Session = session;
            Url = url;
            Project = project;
        }
    }
}