using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace FreshAir_WindowManager_WPF_MVVM
{
    public interface IWindowManager
    {
        List<WindowModel> OpenedViews { get; set; }
        bool? OpenDialogWindow(string messageBoxText);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult);
        bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult, MessageBoxOptions messageBoxOptions);

        string FilePathDialogWindow();
        string FilePathDialogWindow(string messageBoxTitle);
        string FilePathDialogWindow(string messageBoxTitle, string initialDirectory);
        string FilePathDialogWindow(string messageBoxTitle, string initialDirectory, string defaultExtension);

        void OpenWindow(object viewModel);
        Task<bool?> OpenModalDialogWindow(object viewModel);
        Task<object> OpenResultWindow(object viewModel);
        void CloseWindow(object viewModel);
    }
}
