using System.Threading.Tasks;
using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories
{
    public interface ISubCategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// Функція повертає інформацію вказаного вигляду з fs.to 
        /// </summary>
        /// <param name="view">Вигляд повернутої медіа-інформації</param>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків <see cref="MediaListed"/>, якщо вказано <see cref="View.List"/>, або масив деталізованих медіа-списків <see cref="MediaDetailed"/> при <see cref="View.Detailed"/></returns>
        /// <remarks>Повернуте значення потрібно привести до потрібного типу</remarks>
        Task<Media[]> GetMediaAsync(View view, Sort sort, int page = 0);

        /// <summary>
        /// Функція повертає інформацію деталізованим списком з fs.to 
        /// </summary>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив деталізованих медіа-списків</returns>
        Task<MediaDetailed[]> GetDetailedMediaAsync(Sort sort, int page = 0);

        /// <summary>
        /// Функція повертає інформацію списком з fs.to 
        /// </summary>
        /// <param name="sort">Вид сортування медіа</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків</returns>
        Task<MediaListed[]> GetListedMediaAsync(Sort sort, int page = 0);
    }
}