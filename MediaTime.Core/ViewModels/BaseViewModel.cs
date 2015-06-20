using System;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Localization;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using Cirrious.MvvmCross.Plugins.Network.Rest;
using Cirrious.MvvmCross.ViewModels;

namespace MediaTime.Core.ViewModels
{
    public enum Lifecycle
    {
        Run,
        Init,
        Reload,
        ReloadFail,
        Start,
        StartFail,
        Save,
        SaveFail,
    }

    /// <summary>
    ///    Defines the BaseViewModel type.
    /// </summary>
    public abstract class BaseViewModel : MvxViewModel
    {
        protected Lifecycle LifecycleState;
        
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>An instance of the service.</returns>
        public TService GetInstance<TService>() where TService : class
        {
            return Mvx.Resolve<TService>();
        }
        public object GetInstance(Type type)
        {
            return Mvx.Resolve(type);
        }

        //protected void FileDownload(string url)
        //{
        //    var fileDownloader = new MvxHttpFileDownloader();
        //    var fileStore = MvxFileStoreHelper.SafeGetFileStore();
        //    fileStore.EnsureFolderExists("MediaTime");
        //    fileDownloader.RequestDownload(url, fileStore.PathCombine("MediaTime", "123"), Success, Error);
        //    fileStore.WriteFile();
        //}

        //private void Error(Exception exception)
        //{
            
        //}

        //private void Success()
        //{
            
        //}

        #region CIRS Lifecycle
        protected BaseViewModel() 
        {
            LifecycleState = Lifecycle.Run;
        }
        public virtual void Init()
        {
            LifecycleState = Lifecycle.Init;
        }
        //public void ReloadState(ISavedState savedState)
        //{
        //    LifecycleState = Lifecycle.Reload;
        //}
        public override void Start()
        {
            LifecycleState = Lifecycle.Start;
        }
        public virtual ISavedState SaveState()
        {
            LifecycleState = Lifecycle.Save;
            return null;
        }
        #endregion

    }
}