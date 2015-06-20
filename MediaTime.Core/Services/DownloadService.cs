namespace MediaTime.Core.Services
{
    //public class ProgressDowloadFileEventArgs 
    //{
    //    public ulong RecivedBytes { get; set; }
    //    public ulong TotalBytesToRecive { get; set; }
    //    public DownloadInfo Track { get; set; }

    //    public ProgressDowloadFileEventArgs() { }

    //    public ProgressDowloadFileEventArgs(DownloadInfo track, ulong recived, ulong total) 
    //    {
    //        RecivedBytes = recived;
    //        TotalBytesToRecive = total;
    //        Track = track;
    //    }
    //    private static ProgressDowloadFileEventArgs _instance;
        
    //    public static ProgressDowloadFileEventArgs Instance
    //    {
    //        get { return _instance ?? (_instance = new ProgressDowloadFileEventArgs()); }
    //    }
    //}

    ////Клас-контейнер для закачок.

    //public class DownloadService
    //{
    //    private static DownloadService _instance;

    //    public static DownloadService Current
    //    {
    //        get { return _instance ?? (_instance = new DownloadService()); }
    //    }

    //    private readonly List<DownloadInfo> _downloadsList = new List<DownloadInfo>();
    //    public List<DownloadInfo> DownloadsList { get { return _downloadsList; } }

    //    #region Events

    //    public event EventHandler<DownloadInfo> AddToDownloads;
    //    public event EventHandler<DownloadInfo> RemoveFromDownloads;
    //    public event EventHandler<ProgressDowloadFileEventArgs> ProgressChanged;

    //    protected virtual void OnProgressChanged(ProgressDowloadFileEventArgs e)
    //    {
    //        var handler = ProgressChanged;
    //        if (handler != null) handler(this, e);
    //    }

    //    protected virtual void OnRemoveFromDownloads(DownloadInfo e)
    //    {
    //        var handler = RemoveFromDownloads;
    //        if (handler != null) handler(this, e);
    //    }

    //    protected virtual void OnAddToDownloads(DownloadInfo e)
    //    {
    //        var handler = AddToDownloads;
    //        if (handler != null) handler(this, e);
    //    }

    //    #endregion

    //    private SynchronizationContext _synchronizationContext;

    //    //public DownloadInfo CurrentTrack { get; set; }

    //    public async Task<IStorageFile> DownloadAsunc(Track track)
    //    {
    //        using (var hhtpClient = new HttpClient())
    //        {
    //            var response = await hhtpClient.GetAsync(track.Stream);
    //            response.EnsureSuccessStatusCode();

    //            var bytes = await response.HtmlContent.ReadAsByteArrayAsync();
    //            var tempfile =
    //                await
    //                ApplicationData.Current.TemporaryFolder.CreateFileAsync(string.Format("{0} - {1}.mp3", track.Name, track.Artist_Name), CreationCollisionOption.ReplaceExisting);

    //            await FileIO.WriteBytesAsync(tempfile, bytes);
    //            return tempfile;
    //        }
    //    }

    //    public async Task AddToDownloadQueryAsunc(Track track)
    //    {
    //        if (track == null) return;
    //        var saveFile = await PickSaveFaleStorage(track);
    //        if (saveFile == null) return;

    //        var downloaItem = new DownloadInfo
    //            {
    //                Track = track,
    //                StorageFile = saveFile
    //            };
    //        _downloadsList.Add(downloaItem);
    //        OnAddToDownloads(downloaItem);

    //        if (downloaItem.DownloadOperation != null &&
    //            downloaItem.DownloadOperation.Progress.Status != BackgroundTransferStatus.Canceled)
    //            return;
    //        try
    //        {
    //            using (IInputStream stream = new InMemoryRandomAccessStream())
    //            {
    //                var downloader = new BackgroundDownloader();
    //                downloaItem.DownloadOperation =
    //                    await
    //                    downloader.CreateDownloadAsync(new Uri(downloaItem.Track.Stream), downloaItem.StorageFile,
    //                                                   stream);

    //                var progressCallback = new Progress<DownloadOperation>(OnDownloaderProgress);
    //                _synchronizationContext = SynchronizationContext.Current;
    //                downloaItem.CancellationSource = new CancellationTokenSource();

    //                await
    //                    downloaItem.DownloadOperation.StartAsync()
    //                               .AsTask(downloaItem.CancellationSource.Token, progressCallback);

    //                _downloadsList.Remove(downloaItem);
    //                OnRemoveFromDownloads(downloaItem);
    //            }
    //        }
    //            //TODO перехоплення виключень
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //    }

    //    private void OnDownloaderProgress(DownloadOperation downloadOperation)
    //    {
    //        _synchronizationContext.Post(state =>
    //            {
    //                var file =
    //                    _downloadsList.SingleOrDefault(
    //                        f => f.DownloadOperation != null && f.DownloadOperation.Guid == downloadOperation.Guid);
    //                if (file == null) return;

    //                file.TotalBytesToRecive = downloadOperation.Progress.TotalBytesToReceive;
    //                file.RecivedBytes = downloadOperation.Progress.BytesReceived;
                    
    //                ProgressDowloadFileEventArgs.Instance.Track = file;
    //                ProgressDowloadFileEventArgs.Instance.TotalBytesToRecive = file.TotalBytesToRecive;
    //                ProgressDowloadFileEventArgs.Instance.RecivedBytes = file.RecivedBytes;

    //                OnProgressChanged(ProgressDowloadFileEventArgs.Instance);
    //            }, null);
            

    //         //OnProgressChanged(new ProgressDowloadFileEventArgs(CurrentTrack,                                   
    //         //                   downloadOperation.Progress.BytesReceived,
    //         //                                                      downloadOperation.Progress.TotalBytesToReceive)),
    //    }

    //    private static async Task<StorageFile> PickSaveFaleStorage(Track track)
    //    {
    //        try
    //        {
    //            var picker = new FileSavePicker
    //                {
    //                    SuggestedStartLocation = PickerLocationId.MusicLibrary,
    //                    SuggestedFileName = string.Format("{0} - {1}", track.Name, track.Artist_Name)
    //                };
    //            picker.FileTypeChoices.Add("mp3 file", new List<string>{".mp3"});
    //            picker.DefaultFileExtension = ".mp3";
    //            var file = await picker.PickSaveFileAsync();
    //            return file;
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }


    //}
 
}
