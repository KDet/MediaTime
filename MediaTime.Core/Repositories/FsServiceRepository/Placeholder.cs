using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public abstract class Placeholder : IPlaceholder
    {
        protected const string BaseUrl = "http://brb.to";
        protected const string Lang = "ru";
        //private readonly SynchronizationContext _syncContext;
        
        public string Url { get; protected set; }
        protected HtmlDocument CurrentHtmlDocument { get; set; }
        
        protected readonly IHtmlPageLoaderService HtmlPageLoaderService;
        
        protected Placeholder(IHtmlPageLoaderService htmlPageLoaderService)
        {
            HtmlPageLoaderService = htmlPageLoaderService;
            Url = string.Format("{0}/", BaseUrl);
            //Windows 8 only
            //перевірка підключення до інтернету
            //_syncContext = SynchronizationContext.Current;
            //NetworkInformation.NetworkStatusChanged += sender =>
            //    {
            //        var profile = NetworkInformation.GetInternetConnectionProfile();
            //        _syncContext.Post(state =>
            //            {
            //                if (profile == null)
            //                {
            //                    //викликає подію ConnectionLost 
            //                    if (ConnectionLost != null)
            //                        ConnectionLost(this, EventArgs.Empty);
            //                }
            //                else
            //                {
            //                    //викликає подію ConnectionFound 
            //                    if (ConnectionFound != null)
            //                        ConnectionFound(this, EventArgs.Empty);
            //                }
            //            }, null);
            //    };
        }

        /// <summary>
        /// Функція асинхронного пошуку контенту
        /// </summary>
        /// <param name="textToFind">Пошуковий запит</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Перечислення медіа файлів</returns>
        /// <remarks>Функція привязана до Url, в дочірніх класах можна цим управляти. 
        /// Поведінка за замовчуванням - Url відповідає репозиторії з якої викликається функція пошуку.
        /// Сторінки можна перегортати в циклі доти поки результат функції не буте - пуста послідовність</remarks>
        public async Task<SearchMedia[]> SearchAsync(string textToFind, int page = 0)
        {
            var query = string.Format("{0}search.aspx?search={1}&page={2}", Url,
                textToFind.Trim().Replace(' ', '+'), page);

            //ТУТ потрібно завжи загружати сторінку пошуку незалежно від CurrentHtmlDocument
            var html = await HtmlPageLoaderService.LoadPageAsync(query);
            return Search(html).ToArray();
        }
        /// <summary>
        /// Функція повертає всю наявню інформацію про вказане медіа
        /// </summary>
        /// <param name="media">Медіа файл сервісу fs.to</param>
        /// <returns>Клас з розгорнутою інформацією про медіа</returns>
        public async Task<RetrievedMedia> RetrieveMediaAsync(Media media)
        {
            var retrievedMedia = await RetrieveMediaUrlAsync(media.Url);
            if (string.IsNullOrEmpty(retrievedMedia.Image))
                retrievedMedia.Image = ResizeImage(media.Image, ImageSize.Poster);
            if (string.IsNullOrEmpty(retrievedMedia.Title))
                retrievedMedia.Title = media.Title;
            if (string.IsNullOrEmpty(retrievedMedia.SubTitle))
                retrievedMedia.SubTitle = media.SubTitle;
            return retrievedMedia;
        }
        /// <summary>
        /// Функція повертає всю наявню інформацію про вказане медіа
        /// </summary>
        /// <param name="url">Посилання на медіа сервісу fs.to</param>
        /// <returns>Клас з розгорнутою інформацією про медіа</returns>
        private async Task<RetrievedMedia> RetrieveMediaUrlAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            var query = string.Format("{0}?lang={1}?ajax&folder", url, Lang);
            //var query2 = string.Format("{0}?ajax&folder=0", url);
            var html = await HtmlPageLoaderService.LoadPageAsync(query);

            CurrentHtmlDocument = html;
            var generalInfo = HelpGetGeneralInfo(html);
            var media = new RetrievedMedia
            {
                Url = url,
                Title = generalInfo.Title,
                SubTitle = generalInfo.SubTitle,
                Image = ResizeImage(generalInfo.Image, ImageSize.Poster)
            };
            
            var content =
                html.DocumentNode.ChildNodes.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("tab-item-content"));
            if (content != null)
            {
                var descriptionNode =
                    content.ChildNodes.Where(htmlNode => htmlNode.Name == "div")
                        .FirstOrDefault(
                            htmlNode =>
                                htmlNode.GetAttributeValue("class", "none").Contains("description"));
                bool useAlternativeDescription = true;
                if (descriptionNode != null)
                {
                    var description = descriptionNode.Descendants("p").LastOrDefault();
                    if (description != null)
                    {
                        media.Description = description.InnerText;
                        useAlternativeDescription = false;
                    }
                }
                var itemInfoNode =
                    content.Descendants("div")
                        .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("item-info")) ??
                        //другий шанс
                    content.Descendants("div")
                            .LastOrDefault(node => node.GetAttributeValue("class", "none").Contains("actions-panel_type_material"));
                if (itemInfoNode != null)
                {
                    if (useAlternativeDescription)
                    {
                        var description = itemInfoNode.ChildNodes.LastOrDefault(node => node.Name == "p");
                        if (description != null)
                            media.Description = description.InnerText;
                    }
                    media.InfoTable = HelpGetInfoTable(itemInfoNode);
                    var ratesNode = itemInfoNode.Descendants("div")
                        .FirstOrDefault(
                            htmlNode =>
                                htmlNode.GetAttributeValue("class", "none").Contains("vote"));
                    if (ratesNode != null)
                    {
                        var likeNode =
                            ratesNode.Descendants("div")
                                .FirstOrDefault(
                                    node => node.GetAttributeValue("class", "none").Contains("vote-value_type_yes"));
                        if (likeNode != null) media.Likes = SafeStringToNumberConverter(likeNode.InnerText);
                        var dislikeNode = ratesNode.Descendants("div")
                                .FirstOrDefault(
                                    node => node.GetAttributeValue("class", "none").Contains("vote-value_type_no"));
                        if (dislikeNode != null) media.Dislikes = SafeStringToNumberConverter(dislikeNode.InnerText);
                    }
                }
            }
            media.FileList = (await RetrieveMediaFilesAsync(media)).ToArray();
            //media.FileList = HelpGelFileList(html).ToArray();
            media.Screenshots = HelpGetScreenshots(html).ToArray();
            media.Reviews = HelpGetReviews(html).ToArray();
            media.Similar = GetRelatedMedia(html).ToArray();
            return media;
        }

        public async Task<IEnumerable<Storage>> RetrieveMediaFilesAsync(Media media)
        {
            if (string.IsNullOrWhiteSpace(media.Url)) return null;
            var query = string.Format("{0}?ajax&folder", media.Url);
            //var query2 = string.Format("{0}?ajax&folder=0", url);
            var html = await HtmlPageLoaderService.LoadPageAsync(query);
            var fileList = html.DocumentNode;//.Descendants("body").FirstOrDefault();
            return fileList == null ? new Storage[0] : OpenFileList(fileList);
        }

        /// /// <summary>
        /// Функція асинхронного перегляду найбільш популярного контенту
        /// </summary>
        /// <param name="firstRefresh">Оновити сторінку перед отриманням даних</param>
        /// <returns>Перечислення медіа файлів</returns>
        /// <remarks>Функція перед завантаженням сторінки перевіряє чи вже є достуна сторінка.
        /// Проте найпопулярніший контент часто оновляється і в дочірніх класах цю поведінку можна змінити на - загружати сторінку щоразу (без перевірки)</remarks>
        public async Task<Media[]> GetRelatedMediaAsync(bool firstRefresh = false)
        {
            if (CurrentHtmlDocument == null || firstRefresh)
                CurrentHtmlDocument = await HtmlPageLoaderService.LoadPageAsync(Url);
            return GetRelatedMedia(CurrentHtmlDocument).ToArray();
        }
        
        protected IEnumerable<SearchMedia> Search(HtmlDocument html)
        {

            // var table = html.DocumentNode.SelectSingleNode("//div[@class='b-search-results']//div[@class='main']/table");
            var root =
                html.DocumentNode.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("search-results"));
            if (root == null) yield break;

            //буває випадок з двома таблицями, друга - додаткові матеріали (не цікавить, бо пошук ведеться тільки по поточному каталозі)
            //приклад тут http://brb.to/audio/search.aspx?search=%09+%D0%9B%D0%B5%D0%BE%D0%BD%D0%B8%D0%B4+%D0%90%D0%B3%D1%83%D1%82%D0%B8%D0%BD
            var table = root.Descendants("table").FirstOrDefault();
            //якщо нема таблиці з результатами пошуку, то нічого не знайдено
            if (table == null) yield break;

            var trNodes = table.ChildNodes.Where(node => node.Name == "tr");
            foreach (var item in trNodes)
            {
                var tdNodes = item.ChildNodes.Where(node => node.Name == "td" && node.HasChildNodes).ToArray();
                //Результат пошуку завжди складається з 3 блоків (номер, картинка з посиланням, опис)
                //Якщо не так, то було змінено розумітку сайту!!!
                if (tdNodes.Length < 3) continue;

                var searchResalt = new SearchMedia();
                //1 не цікавить
                //2 блок
                var hyperlinkImage = tdNodes[1].ChildNodes.FirstOrDefault(node => node.Name == "a");
                if (hyperlinkImage != null)
                {
                    searchResalt.Url = string.Format("{0}{1}", BaseUrl, hyperlinkImage.Attributes["href"].Value);
                    searchResalt.Title = hyperlinkImage.Attributes["title"].Value;
                    var image = hyperlinkImage.ChildNodes.FirstOrDefault(node => node.Name == "img");
                    if (image != null) searchResalt.Image = image.Attributes["src"].Value;
                }

                var textNode = tdNodes[2].ChildNodes.FirstOrDefault(node => node.Name == "p");
                if (textNode != null) searchResalt.Description = textNode.InnerText;

                var sectionNode =
                    tdNodes[2].ChildNodes.FirstOrDefault(
                        node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("section"));
                if (sectionNode != null) searchResalt.Section = sectionNode.InnerText;

                var subTitleNode = tdNodes[2].Descendants("td").LastOrDefault();
                if (subTitleNode != null) searchResalt.SubTitle = subTitleNode.InnerText;

                //додаткова інформація може бути в різній кількості для різного медіа
                var addition = tdNodes[2].ChildNodes.FirstOrDefault(node => node.Name == "div" && node.HasChildNodes);

                if (addition != null)
                {
                    var genreNode =
                        addition.ChildNodes.FirstOrDefault(
                            node =>
                                node.Name == "span" && node.GetAttributeValue("class", "none").Contains("genre") &&
                                node.HasChildNodes);
                    if (genreNode != null) searchResalt.Genre = genreNode.LastChild.InnerText;

                    var rateNode = addition.ChildNodes.FirstOrDefault(
                        node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("rate"));
                    if (rateNode != null) searchResalt.Rates = SafeStringToNumberConverter(rateNode.InnerText);

                    var commentsNode = addition.ChildNodes.FirstOrDefault(node => node.Name == "a");
                    if (commentsNode != null)
                    {
                        var name =
                            FixHtmlInnerText(commentsNode.InnerText)
                                .Split(' ')
                                .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s) && char.IsDigit(s[0]));
                        searchResalt.Comments = SafeStringToNumberConverter(name);
                    }
                }
                yield return searchResalt;
            }
        }
        protected static int SafeStringToNumberConverter(string rates)
        {
            int result;
            return int.TryParse(rates.Trim(), out result) ? result : 0;
        }
        protected static string FixHtmlInnerText(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? string.Empty : HtmlToPlainText(text);
            ////<p> - позначає потовщення шрифту - видалити
            //var fixText = text.Replace("&nbsp;", " ").Replace("<p>", " ").Replace("</p>", "").Replace("\n", "").Replace("  ", " ").Trim();
            //return fixText;
        }

        protected static string HtmlToPlainText(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        protected IEnumerable<Media> GetRelatedMedia(HtmlDocument html)
        {
            var root =
                html.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("content"))
                    .Select(
                        node =>
                            node.Descendants("div")
                                .FirstOrDefault(
                                    htmlNode =>
                                    {
                                        var attribute = htmlNode.GetAttributeValue("class", "none");
                                        return attribute.Contains("posters") || attribute.Contains("poster-series");
                                    }))
                    .FirstOrDefault();
            //було htmlNode.GetAttributeValue("class", "none").Contains("posters")))
            if (root == null) yield break;

            //var posterNodes = CurrentHtmlDocument.DocumentNode.SelectNodes("//div[contains(@class,'l-content')]//div[contains(@class,'b-posters')]//div[@class='b-poster__wrapper']");
            var posterNodes =
                root.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("poster")).ToArray(); // було Contains("poster__wrapper"));

            //якщо не знайдено постерів, то було змінено розмітку сайту!!
            foreach (var posterNode in posterNodes)
            {
                var topMedia = new Media();
                var hyperlink = posterNode.ChildNodes.FirstOrDefault(node => node.Name == "a");
                if (hyperlink == null) continue;

                //посилання на медіа "під" постером
                topMedia.Url = string.Format("{0}{1}", hyperlink.Attributes["href"].Value.StartsWith("http://")? string.Empty : BaseUrl, hyperlink.Attributes["href"].Value);

                var imageNode = hyperlink.Descendants("span").FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("image-poster"));
                //картринка поміщена в тег стилю, тому посилання на неї потрібно "забрати", оминувше решта staff-у
                //окремий випадок для серіалів і мультсеріалів - картинка в тегу hyperlink
                var imageAttribute = imageNode != null ? imageNode.Attributes["style"] : hyperlink.Attributes["style"];
                var imageQuery = imageAttribute != null ? imageAttribute.Value : string.Empty;
                topMedia.Image = imageQuery.Split('\'').FirstOrDefault(s => s.StartsWith("http://"));

                var titleNode = hyperlink.Descendants("span").FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("title"));
                if (titleNode != null)
                {
                    foreach (var childNode in titleNode.ChildNodes)
                    {
                        //підзаголовок - скорочена назва медіа
                        var attributeVal = childNode.GetAttributeValue("class", "none");
                        if (attributeVal.Contains("short"))
                        {
                            topMedia.SubTitle = childNode.InnerText;
                            continue;
                        }
                        //заголовок - повна назва медіа
                        if (attributeVal.Contains("full"))
                        {
                            topMedia.Title = FixHtmlInnerText(childNode.InnerHtml);
                            continue;
                        }
                        //Окремий випадок для серіалів і мультсеріалів
                        if (childNode.Name == "span" && attributeVal.Contains("title"))
                            topMedia.Title = topMedia.SubTitle = FixHtmlInnerText(childNode.InnerText);
                    }
                }

                yield return topMedia;
            }
        }

        private static Media HelpGetGeneralInfo(HtmlDocument html)
        {
            //var root = html.DocumentNode.SelectSingleNode("//div[contains(@class,'l-content')]");

            var root =
                html.DocumentNode.ChildNodes.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("tab-item-content"));
            if (root == null) return null;

            var media = new Media();
            var titleNode =
                html.DocumentNode.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("head__title"));
            bool useAlternativeTitle = true;
            if (titleNode != null)
            {
                var category = titleNode.ChildNodes.FirstOrDefault(node => node.Name == "b");
                if (category != null)
                {
                    media.SubTitle = category.InnerText;
                    titleNode.RemoveChild(category);
                    media.Title = FixHtmlInnerText(titleNode.InnerText);
                    useAlternativeTitle = false;
                }
            }
            if (useAlternativeTitle)
            {
                var titleGroupNode =
                    root.Descendants("div")
                        .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("title-inner"));

                if (titleGroupNode != null)
                {
                    var nameNode =
                        titleGroupNode.Descendants("span")
                            .FirstOrDefault(node => node.GetAttributeValue("itemprop", "none").Contains("name"));
                    if (nameNode != null) media.Title = FixHtmlInnerText(nameNode.InnerText);
                    var subNameNode =
                        titleGroupNode.Descendants("div")
                            .FirstOrDefault(
                                node => node.GetAttributeValue("itemprop", "none").Contains("alternativeHeadline"));
                    if (subNameNode != null) media.SubTitle = FixHtmlInnerText(subNameNode.InnerText);
                }
            }
            //tab-item-content
            var poster =
                root.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("poster-main"))
                    .Select(
                        node =>
                            node.Descendants("img")
                                .FirstOrDefault()).FirstOrDefault();
            //var poster = root.SelectSingleNode(".//div[@class='poster-main']//img");
            if (poster != null) media.Image = poster.Attributes["src"].Value;

            return media;
        }
       
        private static IEnumerable<Storage> HelpGelFileList(HtmlDocument html)
        {
            //var fileList =
            //    html.DocumentNode.SelectSingleNode(
            //        "//div[@class='b-files-folders']//div[@class='b-filelist']//ul[contains(@class,'filelist')]");
            var fileList = html.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "none").Contains("files-folders"))
                .Select(
                    node =>
                        node.Descendants("ul")
                            .FirstOrDefault(
                                htmlNode =>
                                    htmlNode.GetAttributeValue("class", "none").Contains("filelist"))).FirstOrDefault(); 
            return fileList == null ? new Storage[0] : OpenFileList(fileList);
        }
        private static IEnumerable<Storage> OpenFileList(HtmlNode fileList)
        {
            foreach (var storageItem in fileList.ChildNodes)
            {
                if (storageItem.Name == "li" && storageItem.GetAttributeValue("class", "none").Contains("folder"))
                    yield return OpenFolder(storageItem);

                if (storageItem.Name == "li" && storageItem.GetAttributeValue("class", "none").Contains("file-new"))
                    yield return OpenFile(storageItem);

                if (storageItem.Name == "ul" && storageItem.GetAttributeValue("class", "none").Contains("filelist"))
                    foreach (var file in storageItem.ChildNodes.Where(node => node.Name == "li").Select(OpenFile))
                        yield return file;
            }
        }
        private static Folder OpenFolder(HtmlNode folderNode)
        {
            if (!folderNode.HasChildNodes) return null;
            var folder = new Folder();

            var fileListNode =
                folderNode.Descendants("a")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("folder-filelist"));
            if (fileListNode != null)
                folder.FileListRef = string.Format("{0}{1}", BaseUrl, fileListNode.Attributes["href"].Value);

            var headNode =
                folderNode.Descendants("a")
                    .FirstOrDefault(node => node.GetAttributeValue("name", "none").Contains("fl"));

            string flag = string.Empty, quality = string.Empty;
            if (headNode != null)
            {
                var name = headNode.Attributes["name"];
                folder.Id = name != null ? SafeStringToNumberConverter(name.Value.Replace("fl", "")) : 0;
                folder.Name = FixHtmlInnerText(headNode.InnerText);

                flag = headNode.Attributes["class"].Value.Split(' ').FirstOrDefault(s => s.StartsWith("m-"));
                if (!string.IsNullOrEmpty(flag))
                    flag = flag.Replace("m-", "");
            }

            var folderMarker =
                folderNode.ChildNodes.FirstOrDefault(
                    node => node.Name == "div" && node.GetAttributeValue("class", "none").Contains("folder-mark"));
            if (folderMarker != null)
            {
                quality = folderMarker.Attributes["class"].Value.Split(' ').FirstOrDefault(s => s.StartsWith("m-"));
                if (!string.IsNullOrEmpty(quality))
                    quality = quality.Replace("m-", "");
            }

            folder.Marker = new MarkerSet(quality, flag);

            var sizeNodes =
                folderNode.ChildNodes.Where(
                    node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("size"));

            var sizes = sizeNodes.Select(sizeNode => sizeNode.InnerText).ToArray();
            folder.Size = sizes.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s) && char.IsDigit(s[0]));
            folder.Quality =
                FixHtmlInnerText(sizes.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s) && char.IsLetter(s[0])));

            //<span class="material-details">45&nbsp;файлов</span>
            var detailsNode =
                folderNode.ChildNodes.FirstOrDefault(
                    node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("details"));
            if (detailsNode != null) folder.FileCountInfo = FixHtmlInnerText(detailsNode.InnerText);

            //<span class="material-date">15 июля 2012 в 14:54</span>
            var dateNode =
                folderNode.ChildNodes.FirstOrDefault(
                    node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("date"));
            if (dateNode != null) folder.PublicationDate = dateNode.InnerText;

            var fileList =
                folderNode.Descendants("ul").FirstOrDefault(
                    node => node.GetAttributeValue("class", "none").Contains("filelist") && node.HasChildNodes);
            if (fileList != null)
                folder.Content = OpenFileList(fileList).ToArray();

            return folder;
        }
        private static File OpenFile(HtmlNode liNode)
        {
            if (!liNode.HasChildNodes) return null;
            var file = new File();
            foreach (var childNode in liNode.ChildNodes)
            {
                if (childNode.Name == "a" && childNode.Id.Contains("dl_"))
                {
                    file.Id = SafeStringToNumberConverter(childNode.Id.Replace("dl_", ""));
                    file.Reference = string.Format("{0}{1}", BaseUrl, childNode.Attributes["href"].Value);
                    if (childNode.HasChildNodes)
                    {
                        var size =
                            childNode.ChildNodes.FirstOrDefault(
                                node => node.Name == "span" && node.GetAttributeValue("class", "none").Contains("size"));
                        if (size != null) file.Size = size.InnerText;
                    }
                    continue;
                }
                if (childNode.Name == "a" && childNode.GetAttributeValue("class", "none").Contains("link-material"))
                {
                    var onclick = childNode.Attributes["onclick"];
                    if (onclick != null)
                    {
                        var link = onclick.Value.Split('\'').FirstOrDefault(s => s.Contains("/get/play/"));
                        file.PlayReference = !string.IsNullOrEmpty(link)
                            ? string.Format("{0}{1}", BaseUrl, link)
                            : string.Empty;
                        continue;
                    }
                    var rel = childNode.Attributes["rel"];
                    if (rel != null && !string.IsNullOrEmpty(rel.Value) && rel.Value.Contains("/get/"))
                        file.PlayReference = string.Format("{0}{1}", BaseUrl, rel.Value);
                }
            }
            var name =
                liNode.Descendants("span")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("material-filename-text"));
            if (name != null) file.Name = name.InnerText;

            return file;
        }
        private static IEnumerable<Review> HelpGetReviews(HtmlDocument html)
        {
            //var content = html.DocumentNode.SelectSingleNode("//div[@class='l-content  ']//div[@class='b-user-reviews']//div[@class='content']");
            var content =
                html.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("user-reviews"))
                    .Select(
                        node =>
                            node.Descendants("div")
                                .FirstOrDefault(
                                    htmlNode =>
                                        htmlNode.GetAttributeValue("class", "none").Contains("content")))
                    .FirstOrDefault() ??
                    //другий шанс
                html.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("item-material-comments"))
                    .Select(
                        node =>
                            node.Descendants("div")
                                .FirstOrDefault(
                                    htmlNode =>
                                        htmlNode.GetAttributeValue("class", "none").Contains("content")))
                    .FirstOrDefault();
            if(content==null) yield break;

            foreach (var review in content.ChildNodes.Where(node => node.Name == "div"))
            {
                Review reviewMedia;
                var classAttribute = review.GetAttributeValue("class", "none");
                var idAttribute = review.Id;
                if (classAttribute.Contains("user-review-best"))
                {
                    reviewMedia = new Review();
                    var text =
                        review.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "none").Contains("content"))
                            .Select(
                                node =>
                                    node.Descendants("p")
                                        .FirstOrDefault())
                            .FirstOrDefault();
                    // var text = review.SelectSingleNode(".//div[@class='content']//p");
                    if (text != null) reviewMedia.Text = text.InnerText;

                    //var headInfo = review.SelectSingleNode(".//div[@class='head']//a");
                    var headInfo =
                        review.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "none").Contains("head"))
                            .Select(
                                node =>
                                    node.Descendants("a")
                                        .FirstOrDefault())
                            .FirstOrDefault();
                    if (headInfo != null)
                    {
                        reviewMedia.UserPage = string.Format("{0}{1}", BaseUrl,
                            headInfo.Attributes["href"].Value);
                        var avatar =
                            headInfo.Descendants("span")
                                .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("avatar"));
                        //var avatar = headInfo.SelectSingleNode(".//span[@class='avatar']");
                        if (avatar != null)
                        {
                            var avatarQuery = avatar.Attributes["style"].Value;
                            var avatarImage =
                                avatarQuery.Split('\'').FirstOrDefault(s => s.StartsWith("http://"));
                            reviewMedia.Avatar = ResizeImage(avatarImage, ImageSize.Poster);
                        }

                        var login =
                            headInfo.Descendants("span")
                                .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("login"));
                        //var login = headInfo.SelectSingleNode(".//span[@class='login']");
                        if (login != null) reviewMedia.Login = login.InnerText;
                    }
                    reviewMedia.IsBest = true;
                    yield return reviewMedia;
                    continue;
                }
                if (!string.IsNullOrEmpty(idAttribute))
                {
                    reviewMedia = new Review { Id = idAttribute };
                    //var textNode = review.SelectSingleNode(".//div[@class='text']//p");
                    var textNode =
                        review.Descendants("div")
                            .Where(node => node.GetAttributeValue("class", "none").Contains("text"))
                            .Select(
                                node =>
                                    node.Descendants("p")
                                        .FirstOrDefault())
                            .FirstOrDefault() ??
                            //другий шанс
                        review.Descendants("div")
                            .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("text"));
                    if (textNode != null)
                        reviewMedia.Text = textNode.InnerText;
                    if (FillOldStyleReview(reviewMedia, review))
                        yield return reviewMedia;
                    else
                    {
                        var avatarNode =
                            review.Descendants("a")
                                .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("photo"));
                        if (avatarNode != null)
                        {
                            reviewMedia.UserPage = string.Format("{0}{1}", BaseUrl, avatarNode.Attributes["href"].Value);
                            var avatarQuery = avatarNode.Attributes["style"].Value;
                            var avatarImage =
                                avatarQuery.Split('\'').FirstOrDefault(s => s.StartsWith("http://"));
                            reviewMedia.Avatar = ResizeImage(avatarImage, ImageSize.Thumbnail);
                        }
                        //right-head
                        var userNameNode =
                            review.Descendants("div")
                                .Where(node => node.GetAttributeValue("class", "none").Contains("right-head"))
                                .Select(node => node.Descendants("span").FirstOrDefault()).FirstOrDefault();
                        if (userNameNode != null) 
                            reviewMedia.Login = FixHtmlInnerText(userNameNode.InnerText);

                        var dateNode = review.Descendants("time").FirstOrDefault();
                        if (dateNode != null)
                            reviewMedia.Date = dateNode.Attributes.Contains("datetime")
                                ? dateNode.Attributes["datetime"].Value
                                : FixHtmlInnerText(dateNode.InnerText);

                        yield return reviewMedia;
                    }
                }
            }
        }

        private static bool FillOldStyleReview(Review reviewMedia, HtmlNode reviewNode)
        {
            //var detailNode = review.SelectSingleNode(".//div[@class='details']");
            var detailNode =
                reviewNode.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("details"));
            if (detailNode != null)
            {
                //var leftNode = detailNode.SelectSingleNode(".//div[@class='left']");
                var leftNode =
                    reviewNode.Descendants("div")
                        .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("left"));
                if (leftNode != null)
                {
                    var date = leftNode.ChildNodes.FirstOrDefault(node => node.Name == "span");
                    if (date != null) reviewMedia.Date = date.InnerText;

                    var aNode =
                        leftNode.ChildNodes.FirstOrDefault(node => node.Name == "a" && node.HasChildNodes);
                    if (aNode != null)
                    {
                        reviewMedia.UserPage = string.Format("{0}{1}", BaseUrl, aNode.Attributes["href"].Value);
                        var avatar =
                            aNode.Descendants("span")
                                .FirstOrDefault(
                                    node => node.GetAttributeValue("class", "none").Contains("avatar"));
                        //var avatar = aNode.SelectSingleNode(".//span[@class='avatar']");
                        if (avatar != null)
                        {
                            var avatarQuery = avatar.Attributes["style"].Value;
                            var avatarImage =
                                avatarQuery.Split('\'').FirstOrDefault(s => s.StartsWith("http://"));
                            reviewMedia.Avatar = ResizeImage(avatarImage, ImageSize.Poster);
                        }

                        var login =
                            aNode.Descendants("span")
                                .FirstOrDefault(
                                    node => node.GetAttributeValue("class", "none").Contains("login"));
                        //var login = aNode.SelectSingleNode(".//span[@class='login']");
                        if (login != null) reviewMedia.Login = login.InnerText;
                    }
                }

                //var rightNode = detailNode.SelectSingleNode(".//div[@class='right']");
                //var rightNode =
                //    reviewNode.Descendants("div")
                //        .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("right"));
                //if (rightNode != null)
                //{
                //    var rating =
                //        rightNode.Descendants("span")
                //            .FirstOrDefault(node => node.Id.Contains("vote_value"));
                //    if (rating != null) reviewMedia.Rates = SafeStringToNumberConverter(rating.InnerText);
                //}
                return true;
            }
            return false;
        }

        protected static string ResizeImage(string image, ImageSize size)
        {
            if (string.IsNullOrEmpty(image)) return string.Empty;
            var pic = image.TrimEnd('?', ' ', '\'');
            if (size == ImageSize.Default)
                return pic;
            try
            {
                var end = pic.LastIndexOf('/');
                var start = pic.LastIndexOf('/', end - 1, pic.Length - end - 2);

                var res = pic.Substring(0, start + 1) + (int)size + pic.Substring(end, pic.Length - end);
                return res;
            }
            catch (System.ArgumentException)
            {
                return string.Empty;
            }
        }
        private static IEnumerable<string> HelpGetScreenshots(HtmlDocument html)
        {
            var gallery = html.GetElementbyId("b-screenshots-gallery");
            if (gallery == null) yield break;

            //var pictures = gallery.SelectNodes("//div[@class='scrollable']//div[@class='items']/a");
            var picturesInfo =
                gallery.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("scrollable"))
                    .Select(
                        node =>
                            node.Descendants("div")
                                .FirstOrDefault(
                                    htmlNode =>
                                        htmlNode.GetAttributeValue("class", "none").Contains("items"))).FirstOrDefault();
            if (picturesInfo == null) yield break;

            foreach (var picture in picturesInfo.ChildNodes.Where(node => node.Name == "a"))
                yield return picture.Attributes["rel"].Value;
        }

        private static Dictionary<string, string> HelpGetInfoTable(HtmlNode itemInfoNode)
        {
            Dictionary<string, string> tableDictionary = null;
            //var root = content.SelectSingleNode(".//div[@class='item-info']");
            var root = itemInfoNode;

            //var alternativeRoot = content.SelectSingleNode(".//ul[contains(@class,'b-material-info')]");
            var materialNode =
                root.Descendants("ul")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "none").Contains("material-info"));
            if (materialNode != null)
            {
                tableDictionary =
                    materialNode.ChildNodes.Where(node => node.Name == "li")
                        .ToDictionary(liNode =>
                        {
                            var temp = liNode.Clone();
                            var badNodes = temp.ChildNodes.Where(node => node.Name == "a").ToArray();
                            for (int i = badNodes.Length - 1; i >= 0; i--)
                                temp.RemoveChild(badNodes[i]);
                            return FixHtmlInnerText(temp.InnerText).TrimEnd(':');
                        },
                            liNode =>
                            {
                                var info = new StringBuilder();
                                foreach (var aNode in liNode.ChildNodes.Where(node => node.Name == "a"))
                                    info.AppendFormat("{0} ", FixHtmlInnerText(aNode.InnerText));
                                return info.ToString().Trim();
                            });
            }
            else
            {
                //var tableInfo's = root.SelectNodes(".//table/tr");
                var tableInfo = root.Descendants("table").FirstOrDefault(node => node.HasChildNodes);
                if (tableInfo != null)
                    tableDictionary =
                        tableInfo.ChildNodes.Where(node => node.Name == "tr")
                            .Select(trNode => trNode.ChildNodes.Where(node => node.Name == "td").ToArray())
                            .Where(tdNodes => tdNodes.Length == 2)
                            .ToDictionary(tdNodes => FixHtmlInnerText(tdNodes[0].InnerText).TrimEnd(':'),
                                tdNodes =>
                                {
                                    var badNode =
                                        tdNodes[1].ChildNodes.FirstOrDefault(
                                            node =>
                                                node.Name == "a" &&
                                                node.GetAttributeValue("class", "none")
                                                    .Contains("show-more"));
                                    if (badNode != null)
                                        tdNodes[1].RemoveChild(badNode);
                                    return FixHtmlInnerText(tdNodes[1].InnerText);
                                });

                //var spoilerInfo = root.SelectNodes(".//div[contains(@class,'spoiler')]");
                var spoilerInfo =
                    root.Descendants("div")
                        .Where(node => node.GetAttributeValue("class", "none").Contains("spoiler"));
                var spoilerDictionary =
                    spoilerInfo.Select(divNode => divNode.ChildNodes.Where(node => node.Name == "div").ToArray())
                        .Where(divNode => divNode.Length == 2)
                        .ToDictionary(nodes => FixHtmlInnerText(nodes[0].InnerText).TrimEnd(':'),
                            nodes => FixHtmlInnerText(nodes[1].InnerText));

                if (tableDictionary != null && tableDictionary.Count > 0)
                    foreach (var pair in spoilerDictionary)
                        tableDictionary[pair.Key] = pair.Value;
                else
                    tableDictionary = spoilerDictionary;
            }
            return tableDictionary;
        }
    }
}