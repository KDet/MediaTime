using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaTime.Core.Extensions;
using MediaTime.Core.Model;
using MediaTime.Core.Properties;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters;
using MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories.Filters.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public abstract class BaseMultimediaRepository : BaseRepository, ISubCategoryRepository
    {
        protected BaseMultimediaRepository(IHtmlPageLoaderService htmlPageLoaderService) : base(htmlPageLoaderService)
        {
        }

        protected static IEnumerable<string> GetCustomValues([NotNull] HtmlAgilityPack.HtmlDocument html, CustomFilter customFilter)
        {
            var root =
                html.DocumentNode.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "none").Contains("content"))
                    .Select(
                        node =>
                            node.Descendants("ul")
                                .FirstOrDefault(
                                    htmlNode =>
                                        htmlNode.GetAttributeValue("class", "none")
                                            .Contains(customFilter.ToString().ToLower())))
                    .FirstOrDefault();
            if (root == null) yield break;

            //var translateNode = html.DocumentNode.SelectSingleNode("//div[contains(@class,'l-content')]//ul[@class='m-translation']//div[@class='b-popup']/ul");
            //або
            // var languageNode = html.DocumentNode.SelectSingleNode("//div[contains(@class,'l-content')]//ul[@class='m-language']//div[@class='b-popup']/ul");
            var tableSubNode =
                root.Descendants("div")
                    .FirstOrDefault(
                        node => node.GetAttributeValue("class", "none").Contains("popup") && node.HasChildNodes);
            if (tableSubNode == null) yield break;

            var tableNode =
                tableSubNode.ChildNodes.FirstOrDefault(node => node.Name == "ul" && node.HasChildNodes);
            if (tableNode == null) yield break;

            foreach (var value in tableNode.ChildNodes.Where(node => node.Name == "li").Select(liNode =>
            {
                var aNode =
                    liNode.ChildNodes.FirstOrDefault(
                        htmlNode =>
                            htmlNode.Name == "a" && !htmlNode.GetAttributeValue("class", "none").Contains("more"));
                return aNode != null ? aNode.InnerText : null;
            }).Where(s => !string.IsNullOrWhiteSpace(s)))
                yield return value;
        }

        /// <summary>
        /// Функція повертає інформацію вказаного вигляду з fs.to 
        /// </summary>
        /// <param name="view">Вигляд повернутої медіа-інформації</param>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків <see cref="MediaListed"/>, якщо вказано <see cref="View.List"/>, або масив деталізованих медіа-списків <see cref="MediaDetailed"/> при <see cref="View.Detailed"/></returns>
        /// <remarks>Повернуте значення потрібно привести до потрібного типу</remarks>
        public async Task<Media[]> GetMediaAsync(View view, Sort sort, int page = 0)
        {
            return view == View.Detailed
                ? (Media[]) (await GetDetailedMediaAsync(sort, page))
                : await GetListedMediaAsync(sort, page);
        }
        /// <summary>
        /// Функція повертає інформацію деталізованим списком з fs.to 
        /// </summary>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив деталізованих медіа-списків</returns>
        public async Task<MediaDetailed[]> GetDetailedMediaAsync(Sort sort, int page = 0)
        {
            //(Sort)((int)sort) - як би не мінявся default він буде таким як треба
            var query = string.Format("{0}?sort={1}&view=detailed&page={2}", Url,
                ((Sort) ((int) sort)).ToString().ToLower(), page);
            var doc = await HtmlPageLoaderService.LoadPageAsync(query);
            return ProcessDetailedMedia(doc).ToArray();
        }
        /// <summary>
        /// Функція повертає інформацію списком з fs.to 
        /// </summary>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків</returns>
        public async Task<MediaListed[]> GetListedMediaAsync(Sort sort, int page = 0)
        {
            var query = string.Format("{0}?sort={1}&view=list&page={2}", Url,
                ((Sort) ((int) sort)).ToString().ToLower(), page);
            var doc = await HtmlPageLoaderService.LoadPageAsync(query);
            return ProcessListedMedia(doc).ToArray();
        }
        
        //повертає стрічку-запит для вказатого типу відображення медіа (на fs.to) List або Detailed
        protected string HelpComputeQuery(View view, IFilters filters, Sort sort, int page)
        {
            string language = string.Empty, transtale = string.Empty;
            var subQuery = new System.Text.StringBuilder("fl");
            //на випадок, якщо вибрано лише Custom параметри
            bool onlyCustom = true;
            
            //якщо є хоча б один фільтр
            if (filters.HasFilter())
            {
                //фільтри - це властивості заксу з IFilters інтерфейсом
                // var filtersProperties = filters.GetType().GetRuntimeProperties();
                foreach (var filtersProperty in filters.Filters)
                {
                    //особливий випадок - є властивість з іншою (не типовою - не укр чи рус) вказаною мовою
                    if (filtersProperty.Key.Contains("LanguageCustom"))
                    {
                        language = filtersProperty.Value as string;
                        if (!string.IsNullOrWhiteSpace(language) && language != "None")
                            language = string.Format("/language_custom_{0}", language.Replace(' ', '+').Trim());
                    }
                        //особливий випадок - є авторський переклад
                    else if (filtersProperty.Key.Contains("TranslateCustom"))
                    {
                        transtale = filtersProperty.Value as string;
                        if (!string.IsNullOrWhiteSpace(transtale) && transtale != "None")
                            transtale = string.Format("/translate_custom_{0}", transtale.Replace(' ', '+').Trim());
                    }
                    else
                    {
                        //всі решта фільтри, якщо вони є (не none) перечислюються через _
                        var value = filtersProperty.Value.ToString();
                        if (!string.IsNullOrEmpty(value) &&
                            string.Compare(value, "None", System.StringComparison.OrdinalIgnoreCase) != 0)
                        {
                            subQuery.AppendFormat("_{0}", value.ToLower());
                            onlyCustom = false;
                        }
                    }
                }
                //якщо лише користувацькі параметри, то по звичайних нема що шукати
                if (onlyCustom)
                    subQuery = subQuery.Clear();
            }
            else
                //якщо нема фільрів, то нема що шукати
                subQuery = subQuery.Clear();

            //випадок коли буде в запиті два сивмоли // підряд - стерти один /.
            var baseQuery = onlyCustom ? Url.Remove(Url.Length - 1, 1) : Url;

            //кінцевий запит
            var query = string.Format("{0}{1}{2}{3}/?sort={4}&view={5}&page={6}", baseQuery, subQuery, language == "None" ? string.Empty: language, transtale == "None" ? string.Empty : transtale, ((Sort)((int)sort)).ToString().ToLower().Replace("on", ""), ((View)((int)view)).ToString().ToLower().Replace("on", ""), page);

            return query;
        }
    }
}