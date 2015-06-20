using System.Threading.Tasks;
using HtmlAgilityPack;
using MediaTime.Core.Repositories.FsServiceRepository;

namespace MediaTime.Core.Extensions
{
    public static class CustomWebExtensions
    {
        /// <summary>
        /// Begins the process of downloading an internet resource
        /// </summary>
        /// <param name="htmlPageLoaderService"></param>
        /// <param name="url">Url to the html document</param>
        public static async Task<HtmlDocument> LoadPageAsync(
            this IHtmlPageLoaderService htmlPageLoaderService, string url)
        {
            var doc = new HtmlDocument();
            await htmlPageLoaderService.LoadAsync(url);
            doc.LoadHtml(htmlPageLoaderService.HtmlContent);
            return doc;
        }
    }
}