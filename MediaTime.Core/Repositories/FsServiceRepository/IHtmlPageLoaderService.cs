using System;
using System.Threading.Tasks;
using MediaTime.Core.Services;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public interface IHtmlPageLoaderService
    {
        Task<string> LoadAsync(string url);
        string HtmlContent { get; }
        event EventHandler<PageLoaderErrorEventArgs> PageLoaderError;
    }
}