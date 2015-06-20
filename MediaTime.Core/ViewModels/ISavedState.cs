namespace MediaTime.Core.ViewModels
{
    public interface ISavedState
    {
        void ClearCash();
    }

    public interface INavigatedObject
    {
        bool IsObjectValid();
    }
}