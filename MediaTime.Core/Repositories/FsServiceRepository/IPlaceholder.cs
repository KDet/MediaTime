using System.Threading.Tasks;
using MediaTime.Core.Model;

namespace MediaTime.Core.Repositories.FsServiceRepository
{
    public interface IPlaceholder
    {
        string Url { get; }

        /// <summary>
        /// Функція асинхронного пошуку контенту
        /// </summary>
        /// <param name="textToFind">Пошуковий запит</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Перечислення медіа файлів</returns>
        /// <remarks>Функція привязана до Url, в дочірніх класах можна цим управляти. 
        /// Поведінка за замовчуванням - Url відповідає репозиторії з якої викликається функція пошуку.
        /// Сторінки можна перегортати в циклі доти поки результат функції не буте - пуста послідовність</remarks>
        Task<SearchMedia[]> SearchAsync(string textToFind, int page = 0);

        /// <summary>
        /// Функція повертає всю наявню інформацію про вказане медіа
        /// </summary>
        /// <param name="media">Медіа файл сервісу fs.to</param>
        /// <returns>Клас з розгорнутою інформацією про медіа</returns>
        Task<RetrievedMedia> RetrieveMediaAsync(Media media);

        ///// <summary>
        ///// Функція повертає всю наявню інформацію про вказане медіа
        ///// </summary>
        ///// <param name="url">Посилання на медіа сервісу fs.to</param>
        ///// <returns>Клас з розгорнутою інформацією про медіа</returns>
        //Task<RetrievedMedia> RetrieveMediaUrlAsync(string url);

        /// /// <summary>
        /// Функція асинхронного перегляду найбільш популярного контенту
        /// </summary>
        /// <param name="firstRefresh">Оновити сторінку перед отриманням даних</param>
        /// <returns>Перечислення медіа файлів</returns>
        /// <remarks>Функція перед завантаженням сторінки перевіряє чи вже є достуна сторінка.
        /// Проте найпопулярніший контент часто оновляється і в дочірніх класах цю поведінку можна змінити на - загружати сторінку щоразу (без перевірки)</remarks>
        Task<Media[]> GetRelatedMediaAsync(bool firstRefresh = false);
    }
}