﻿using MediaTime.Core.Model;
using MediaTime.Core.Repositories.FsServiceRepository.Enums;

namespace MediaTime.Core.Repositories.FsServiceRepository.MediaRepositories
{
    public interface ICategoryRepository : IPlaceholder
    {
        /// <summary>
        /// Функція повертає інформацію вказаного вигляду з fs.to 
        /// </summary>
        /// <param name="view">Вигляд повернутої медіа-інформації</param>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків <see cref="MediaListed"/>, якщо вказано <see cref="View.List"/>, або масив деталізованих медіа-списків <see cref="MediaDetailed"/> при <see cref="View.Detailed"/></returns>
        /// <remarks>Повернуте значення потрібно привести до потрібного типу</remarks>
        System.Threading.Tasks.Task<Media[]> GetMediaAsync(View view, int page = 0);

        /// <summary>
        /// Функція повертає інформацію деталізованим списком з fs.to 
        /// </summary>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив деталізованих медіа-списків</returns>
        System.Threading.Tasks.Task<MediaDetailed[]> GetDetailedMediaAsync(int page = 0);

        /// <summary>
        /// Функція повертає інформацію списком з fs.to 
        /// </summary>
        /// <param name="page">Сторінка з результатами</param>
        /// <returns>Масив медіа-списків</returns>
        System.Threading.Tasks.Task<MediaListed[]> GetListedMediaAsync(int page = 0);
    }
}