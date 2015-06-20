using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public abstract class BaseRepository : Placeholder
    {
        protected BaseRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
        }

        /// <summary>
        /// Функція повертає інформацію вказаного вигляду з fs.to 
        /// </summary>
        /// <param name="view">Вигляд повернутої медіа-інформації</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків <see cref="MediaListed"/>, якщо вказано <see cref="View.List"/>, або масив деталізованих медіа-списків <see cref="MediaDetailed"/> при <see cref="View.Detailed"/></returns>
        /// <remarks>Повернуте значення потрібно привести до потрібного типу</remarks>
        public async Task<Media[]> GetMediaAsync(View view, int page = 0)
        {
            return view == View.Detailed
                ? (Media[])(await GetDetailedMediaAsync(page))
                : await GetListedMediaAsync(page);
        }
        /// <summary>
        /// Функція повертає інформацію деталізованим списком з fs.to 
        /// </summary>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив деталізованих медіа-списків</returns>
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(int page = 0)
        {
            var query = string.Format("{0}?view=detailed&page={1}", Url, page);
            var doc = await HtmlPageLoaderService.LoadPageAsync(query);
            return ProcessDetailedMedia(doc).ToArray();
        }
        /// <summary>
        /// Функція повертає інформацію списком з fs.to 
        /// </summary>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків</returns>
        public async Task<MediaListed[]> GetListedMediaAsync(int page = 0)
        {
            var query = string.Format("{0}?view=list&page={1}", Url, page);
            HtmlAgilityPack.HtmlDocument doc = await HtmlPageLoaderService.LoadPageAsync(query);
            return ProcessListedMedia(doc).ToArray();
        }

        protected IEnumerable<MediaDetailed> ProcessDetailedMedia(HtmlAgilityPack.HtmlDocument html)
        {
            var root =
                html.DocumentNode.ChildNodes.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("content"));
            if (root == null) yield break;

            CurrentHtmlDocument = html;

            var table =
                root.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("section-list"))
                    .Select(node => node.ChildNodes.FirstOrDefault(htmlNode => htmlNode.Name == "table"))
                    .FirstOrDefault();
            if (table == null) yield break;

            var trNodes = table.ChildNodes.Where(node => node.Name == "tr" && node.HasChildNodes);
            foreach (var item in trNodes)
            {
                var tdNodes = item.ChildNodes.Where(node => node.Name == "td" && node.HasChildNodes);
                foreach (var tdNode in tdNodes)
                {
                    var posterNode = tdNode.ChildNodes.FirstOrDefault(node => node.Name == "div" && node.GetAttributeValue("class", "none").Contains("poster"));
                    if (posterNode == null) continue;

                    var detailedMedia = new MediaDetailed();

                    var linkNode =
                        posterNode.Descendants("a").FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("link"));
                   if (linkNode != null)
                        detailedMedia.Url = string.Format("{0}{1}", BaseUrl, linkNode.Attributes["href"].Value);
                    
                    var spanNodes = posterNode.Descendants("span");
                    foreach (var spanNode in spanNodes)
                    {
                        var attributeVal = spanNode.GetAttributeValue("class", "none");
                        if (attributeVal.Contains("image"))
                        {
                            var imageNode = spanNode.Descendants("img").FirstOrDefault();
                            if (imageNode != null)
                                detailedMedia.Image = imageNode.Attributes["src"].Value;
                        }
                        else if (attributeVal.EndsWith("info-qualities"))
                        {
                            var qualityNodes =
                                spanNode.Descendants()
                                    .Where(node => node.GetAttributeValue("class", "none").Contains("quality"));
                            foreach (var qualityNode in qualityNodes)
                                detailedMedia.Qualities.Add(qualityNode.Attributes["class"].Value.Replace("quality m-",
                                    string.Empty));
                        }
                        else if (attributeVal.EndsWith("vote-positive"))
                        {
                            detailedMedia.Likes = SafeStringToNumberConverter(spanNode.InnerText);
                        }
                        else if (attributeVal.EndsWith("vote-negative"))
                        {
                            detailedMedia.Dislikes = SafeStringToNumberConverter(spanNode.InnerText);
                        }
                        else if (attributeVal.EndsWith("title"))
                        {
                            detailedMedia.Title = detailedMedia.SubTitle = spanNode.InnerText.Trim();
                        }
                        else if (attributeVal.EndsWith("description"))
                        {
                            detailedMedia.Description = spanNode.InnerText.Trim();
                        }
                        else if (attributeVal.EndsWith("field"))
                        {
                            var filed = spanNode.InnerText.Trim();
                            if (!string.IsNullOrWhiteSpace(filed) && !detailedMedia.InfoFields.Contains(filed))
                                detailedMedia.InfoFields.Add(filed);
                        }
                    }
                    yield return detailedMedia;
                }
            }
        }
        protected IEnumerable<MediaListed> ProcessListedMedia(HtmlAgilityPack.HtmlDocument html)
        {
            var root =
                html.DocumentNode.ChildNodes.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("content"));
            if (root == null) yield break;

            CurrentHtmlDocument = html;

            var table =
                root.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("section-list"))
                    .Select(node => node.ChildNodes.FirstOrDefault(htmlNode => htmlNode.Name == "table"))
                    .FirstOrDefault();
            if (table == null) yield break;

            var trNodes = table.ChildNodes.Where(node => node.Name == "tr" && node.HasChildNodes);
            foreach (var item in trNodes)
            {
                var tdNodes = item.ChildNodes.Where(node => node.Name == "td" && node.HasChildNodes);
                foreach (var tdNode in tdNodes)
                {
                    var posterNode = tdNode.ChildNodes.FirstOrDefault(node => node.Name == "div" && node.GetAttributeValue("class", "none").Contains("poster"));
                    if (posterNode == null) continue;

                    var listedMedia = new MediaListed();

                    var linkNode =
                        posterNode.Descendants("a").FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("link"));
                    if (linkNode != null)
                        listedMedia.Url = string.Format("{0}{1}", BaseUrl, linkNode.Attributes["href"].Value);

                    var spanNodes = posterNode.Descendants("span");
                    foreach (var spanNode in spanNodes)
                    {
                        var attributeVal = spanNode.GetAttributeValue("class", "none");
                        if (attributeVal.Contains("image"))
                        {
                            var imageNode = spanNode.Descendants("img").FirstOrDefault();
                            if (imageNode != null)
                                listedMedia.Image = imageNode.Attributes["src"].Value;
                        }
                        else if (attributeVal.Contains("title-short"))
                        {
                            listedMedia.SubTitle = spanNode.InnerText.Trim();
                        }
                        else if (attributeVal.Contains("title-full"))
                        {
                            listedMedia.Title = spanNode.InnerText.Trim();
                        }
                        else if (attributeVal.EndsWith("info-items"))
                        {
                            var filed = spanNode.InnerText.Trim();
                            if (!string.IsNullOrWhiteSpace(filed) && !listedMedia.InfoFields.Contains(filed))
                                listedMedia.InfoFields.Add(filed);
                        }
                        else if (attributeVal.EndsWith("vote-positive"))
                        {
                           listedMedia.Likes = SafeStringToNumberConverter(spanNode.InnerText);
                        }
                        else if (attributeVal.EndsWith("vote-negative"))
                        {
                            listedMedia.Dislikes = SafeStringToNumberConverter(spanNode.InnerText); 
                        }
                        else if (attributeVal.EndsWith("qualities"))
                        {
                            var qualityNodes =
                                spanNode.Descendants()
                                    .Where(node => node.GetAttributeValue("class", "none").Contains("quality"));
                            foreach (var qualityNode in qualityNodes)
                                listedMedia.Qualities.Add(qualityNode.Attributes["class"].Value.Replace("quality m-",
                                    string.Empty));
                        }
                    }
                    yield return listedMedia;
                }
            }
        }
    }
}
