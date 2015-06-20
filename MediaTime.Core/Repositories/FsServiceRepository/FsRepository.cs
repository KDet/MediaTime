using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.AudioRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.GamesRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.TextRepository;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.VideoRepository;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    /// <summary>
    /// Basic repository
    /// </summary>
    public class FsRepository : Placeholder, IFsRepository
    {
        private static VideoRepository _video;
        private static AudioRepository _audio;
        private static TextsRepository _texts;
        private static GamesRepository _games;

        public FsRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
        }

        /// <summary>
        /// Video repository
        /// </summary>
        public VideoRepository Video
        {
            get { return _video ?? (_video = new VideoRepository(HtmlPageLoaderService)); }
        }
        /// <summary>
        /// Audio repository
        /// </summary>
        public AudioRepository Audio
        {
            get { return _audio ?? (_audio = new AudioRepository(HtmlPageLoaderService)); }
        }
        /// <summary>
        /// Literature repository
        /// </summary>
        public TextsRepository Texts
        {
            get { return _texts ?? (_texts = new TextsRepository(HtmlPageLoaderService)); }
        }
        /// <summary>
        /// Game repository
        /// </summary>
        public GamesRepository Games
        {
            get { return _games ?? (_games = new GamesRepository(HtmlPageLoaderService)); }
        }

        private static IEnumerable<UpdatedMedia> Updates(HtmlAgilityPack.HtmlDocument html)
        {
            //гілка з таблицею
            //var trNodes = html.DocumentNode.SelectNodes("//div[@class='b-catalog-items']//div[@class='catalog-new-content']/table//tr");
            var root =
                html.DocumentNode.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("catalog-new-content"));
            if (root == null) yield break;

            //рядки таблиці
            var trNodes = root.Descendants("tr");

            //Проходження по рядках таблиці
            foreach (var trNode in trNodes)
            {
                var updatedMedia = new UpdatedMedia();
                //Проходження по стовпцях
                foreach (var tdNode in trNode.ChildNodes.Where(node => node.Name == "td"))
                {
                    var attribute = tdNode.GetAttributeValue("class", "none");
                    if (attribute.Contains("category"))
                    {
                        var aNode = tdNode.ChildNodes.FirstOrDefault(node => node.Name == "a");
                        if (aNode != null) updatedMedia.SubTitle = aNode.InnerText;
                        continue;
                    }
                    if (attribute.Contains("date"))
                    {
                        updatedMedia.Date = tdNode.InnerText;
                        continue;
                    }
                    if (attribute.Contains("time"))
                    {
                        updatedMedia.Time = tdNode.InnerText;
                        continue;
                    }
                    if (attribute == "none" && tdNode.HasChildNodes)
                    {
                        var aNode = tdNode.ChildNodes.FirstOrDefault(node => node.Name == "a");
                        if (aNode != null)
                        {
                            updatedMedia.Title = aNode.InnerText;
                            updatedMedia.Url = string.Format("{0}{1}", BaseUrl, aNode.Attributes["href"].Value);
                        }
                    }
                }
                yield return updatedMedia;
            }
        }

        /// <summary>
        /// Get fs.to updates list
        /// </summary>
        /// <param name="page">Current page number</param>
        /// <returns>List of new files fs.to service</returns>
        public async Task<UpdatedMedia[]> GetUpdatesAsync(int page = 0)
        {
            //Для списку нещодавних оновлень є власна сторінка. 
            //ЇЇ потрібно завантажувати щоразу при перегляді оновок.
            var query = string.Format("{0}/updates.html?page={1}", BaseUrl, page);
            var html = await HtmlPageLoaderService.LoadPageAsync(query);
            return Updates(html).ToArray();
        }
    }
}