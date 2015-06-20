//using System;
//using System.Collections.Generic;
//using VideoPocket.Core.Repositories;

//namespace MediaOnTouch.ViewModel
//{
//    public class BaseViewModel : BindableBase
//    {
//        public virtual void LoadState(object navigationParameter, Dictionary<string, object> pageState) { }
//        public virtual void SaveState(Dictionary<string, object> pageState) { }

//        //public IObservableMap<string, object> CurentViewModel { get; set; }

//        protected readonly FsRepository FsRepository = new FsRepository();

//        public BaseViewModel()
//        {
//            //_translation = (ResourceDictionary)((ResourceDictionary)Application.Current.Resources["Translator"])[App.LanguageDicionary];
//            FsRepository.ConnectionLost += RepositoryOnConnectionLost;
//            FsRepository.ConnectionFound += RepositoryOnConnectionFound;
            
//            //RefreshPage = new RelayCommand(OnRefreshPage);
//            //TileSelectedCommand = new RelayCommand<MusicBase>(OnTitleSelected);

//            //SearchPane.GetForCurrentView().PlaceholderText = "Search in tracks, albums, artists";
//            //SearchPane.GetForCurrentView().ShowOnKeyboardInput = true;
//        }

//        //#region RelayCommands
 
        

//        //#endregion


//        #region Repository events handlers

//        public virtual void RepositoryOnConnectionFound(object sender, EventArgs eventArgs)
//        {
//            //OnRefreshPage();
//        }

//        public virtual void RepositoryOnConnectionLost(object sender, EventArgs eventArgs)
//        {
//            Notify("Немає з'єднання з інтернетом", "Error");
//        }

//        #endregion


//        #region Navigation

//        public INavigationService NavigationService { get; set; }

//        public virtual bool CanGoBack
//        {
//            get { return NavigationService != null && NavigationService.CanGoBack; }
//        }

//        public virtual bool CanGoForward
//        {
//            get { return NavigationService != null && NavigationService.CanGoForward; }
//        }

//        #endregion

//        protected virtual void Notify(string message, string header)
//        {
//            var messageDialog = new MessageDialog(string.Empty) { HtmlContent = message };
//            if (!string.IsNullOrEmpty(header)) messageDialog.Title = header;
//            // messageDialog.ShowAsync();
//        }
//    }
//}
