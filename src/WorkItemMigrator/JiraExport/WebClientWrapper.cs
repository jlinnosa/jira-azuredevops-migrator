using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JiraExport
{
    internal class WebClientWrapper : IDisposable
    {
        private readonly WebClient _webClient;
        private readonly JiraProvider _jira;

        public WebClientWrapper(JiraProvider jira)
        {
            _jira = jira;
            _webClient = new WebClient();
            _webClient.DownloadFileCompleted += _webClient_DownloadFileCompleted;
        }

        void _webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var completionSource = e.UserState as TaskCompletionSource<object>;

            if (completionSource != null)
            {
                if (e.Cancelled)
                {
                    completionSource.TrySetCanceled();
                }
                else if (e.Error != null)
                {
                    completionSource.TrySetException(e.Error);
                }
                else
                {
                    completionSource.TrySetResult(null);
                }
            }
        }

        public Task DownloadAsync(string url, string fileName)
        {
            _webClient.CancelAsync();

            var completionSource = new TaskCompletionSource<object>();
            _webClient.Headers.Remove(HttpRequestHeader.Authorization);
            _webClient.DownloadFileAsync(new Uri(url), fileName, completionSource);

            return completionSource.Task;
        }

        public Task DownloadWithAuthenticationAsync(string url, string fileName)
        {
            if (String.IsNullOrEmpty(_jira.Settings.Session))
            {
                throw new InvalidOperationException("Unable to download file, user and/or password are missing. You can specify credentials in the configuration file");
            }

            _webClient.CancelAsync();

            var completionSource = new TaskCompletionSource<object>();

            _webClient.Headers.Add("Cookie", $"JSESSIONID={_jira.Settings.Session}");
            _webClient.DownloadFileAsync(new Uri(url), fileName, completionSource);

            return completionSource.Task;
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _webClient.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
