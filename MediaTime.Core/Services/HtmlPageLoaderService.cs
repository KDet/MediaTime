using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using MediaTime.Core.Repositories.FsServiceRepository;

namespace MediaTime.Core.Services
{
    public class HtmlPageLoaderService : IHtmlPageLoaderService
    {
        public event EventHandler<PageLoaderErrorEventArgs> PageLoaderError;
        protected virtual void OnPageLoaderError(PageLoaderErrorEventArgs e)
        {
            var handler = PageLoaderError;
            if (handler != null) handler(this, e);
        }
        public string HtmlContent { get; private set; }
        public async Task<string> LoadAsync(string url)
        {
            var htmlContent = string.Empty;
            await MakeRequest(url, 
                content => htmlContent = content, 
                exception => OnPageLoaderError(new PageLoaderErrorEventArgs(exception, url)));
            HtmlContent = htmlContent;
            return htmlContent;
        }
        public static async Task MakeRequest(string requestUrl, Action<string> successAction, Action<Exception> errorAction)
        {
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(requestUrl);
                var response =
                    await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null);
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                    if (successAction != null)
                        successAction(reader.ReadToEnd());
            }
            catch (Exception ex)
            {
                if (request != null)
                    Mvx.Error("ERROR: '{0}' when making {1} request to {2}", ex.Message, request.Method,
                        request.RequestUri.AbsoluteUri);
                else
                    Mvx.Error("ERROR: '{0}'", ex.Message);
                if (errorAction != null)
                    errorAction(ex);
            }
        }
    }

    public class PageLoaderErrorEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }
        public string Url { get; private set; }
        public PageLoaderErrorEventArgs(Exception exception, string url)
        {
            Exception = exception;
            Url = url;
        }
    }
}