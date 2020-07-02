using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FreshAir_WindowManager_WPF_MVVM
{
    public class WindowManager : IWindowManager
    {
        /// <summary>
        /// Constructor, creates instance of OpenedViews list if not created yet.
        /// </summary>
        public WindowManager()
        {
            if (OpenedViews == null)
            {
                OpenedViews = new List<WindowModel>();
            }
        }

        /// <summary>
        /// List of currently opened WindowModels.
        /// </summary>
        public List<WindowModel> OpenedViews { get; set; }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText)
        {
            return OpenDialogWindowExecute(messageBoxText, "Message box", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle)
        {
            return OpenDialogWindowExecute(messageBoxText, messageBoxTitle, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton)
        {
            return OpenDialogWindowExecute(messageBoxText, messageBoxTitle, messageBoxButton, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
        {
            return OpenDialogWindowExecute(messageBoxText, messageBoxTitle, messageBoxButton, messageBoxImage, MessageBoxResult.None, MessageBoxOptions.None);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult)
        {
            return OpenDialogWindowExecute(messageBoxText, messageBoxTitle, messageBoxButton, messageBoxImage, messageBoxResult, MessageBoxOptions.None);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindow(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult, MessageBoxOptions messageBoxOptions)
        {
            return OpenDialogWindowExecute(messageBoxText, messageBoxTitle, messageBoxButton, messageBoxImage, messageBoxResult, messageBoxOptions);
        }

        /// <summary>
        /// MessageBoxResult wrapper.
        /// </summary>
        /// <returns>Nullable bool</returns>
        public bool? OpenDialogWindowExecute(string messageBoxText, string messageBoxTitle, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage, MessageBoxResult messageBoxResult, MessageBoxOptions messageBoxOptions)
        {
            bool? result = null;

            MessageBoxResult messageResult = MessageBox.Show(messageBoxText, messageBoxTitle, messageBoxButton, messageBoxImage, messageBoxResult, messageBoxOptions);

            switch (messageResult)
            {
                case MessageBoxResult.Yes:
                    result = true;
                    break;
                case MessageBoxResult.No:
                    result = false;
                    break;
                case MessageBoxResult.Cancel:
                    result = null;
                    break;
            }

            return result;
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <returns>String file path</returns>
        public string FilePathDialogWindow()
        {
            return FilePathDialogWindowExecute(null, null, null);
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <returns>String file path</returns>
        public string FilePathDialogWindow(string messageBoxTitle)
        {
            return FilePathDialogWindowExecute(messageBoxTitle, null, null);
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <returns>String file path</returns>
        public string FilePathDialogWindow(string messageBoxTitle, string initialDirectory)
        {
            return FilePathDialogWindowExecute(messageBoxTitle, initialDirectory, null);
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <returns>String file path</returns>
        public string FilePathDialogWindow(string messageBoxTitle, string initialDirectory, string defaultExtension)
        {
            return FilePathDialogWindowExecute(messageBoxTitle, initialDirectory, defaultExtension);
        }

        /// <summary>
        /// OpenFileDialog wrapper.
        /// </summary>
        /// <returns>String file path</returns>
        public string FilePathDialogWindowExecute(string messageBoxTitle, string initialDirectory, string defaultExtension)
        {
            string result = initialDirectory;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = "." + defaultExtension, // Default file extension
                Filter = "Text documents (." + defaultExtension + ")| *." + defaultExtension, // Filter files by extension
                InitialDirectory = initialDirectory,
                RestoreDirectory = true
            };

            bool? dialog = saveFileDialog.ShowDialog();

            // Process save file dialog box results
            if (dialog == true)
            {
                // Save document
                result = saveFileDialog.FileName;
            }

            return result;
        }

        /// <summary>
        /// Opens window for specific view model using name convention. Attaches event handler for Window.Closed.
        /// </summary>
        /// <param name="viewModel">View model name</param>
        public void OpenWindow(object viewModel)
        {
            if (!CheckIfAlreadyOpened(viewModel))
            {
                WindowModel model = CreateWindoModel(viewModel);

                model.OpenedWindow.Closed += new EventHandler((s, e) => WindowClosed(s, e, model));
                model.OpenedWindow.Show();
            }
        }

        /// <summary>
        /// Opens new window for specific view model using name convention. Window may return object. Attaches event handler for Window.Closed.
        /// </summary>
        /// <param name="viewModel">View model name</param>
        /// <returns>Object</returns>
        public async Task<object> OpenResultWindow(object viewModel)
        {
            if (!CheckIfAlreadyOpened(viewModel))
            {
                WindowModel model = CreateWindoModel(viewModel);

                model.OpenedWindow.Closed += new EventHandler((s, e) => ResultWindowClosed(s, e, model));
                model.OpenedWindow.ShowDialog();

                while (!model.IsValueReturned)
                    await Task.Delay(100);

                return model.ReturnedObjectResult;
            }

            return null;
        }

        /// <summary>
        /// Opens new modal dialog window for specific view model using name convention. Window may return nullable bool. Attaches event handler for Window.Closed.
        /// </summary>
        /// <param name="viewModel">View model name</param>
        /// <returns>Nullable bool</returns>
        public async Task<bool?> OpenModalDialogWindow(object viewModel)
        {
            if (!CheckIfAlreadyOpened(viewModel))
            {
                WindowModel model = CreateWindoModel(viewModel);

                model.OpenedWindow.Closed += new EventHandler((s, e) => DialogWindowClosed(s, e, model));
                model.OpenedWindow.ShowDialog();

                while (!model.IsValueReturned)
                    await Task.Delay(100);

                return model.ReturnedDialogResult;
            }

            return null;
        }

        /// <summary>
        /// Extracts window name from passed view model object and adds new WindowModel to OpenedViews list.
        /// </summary>
        /// <param name="viewModel">View model object</param>
        /// <returns>WindowModel object</returns>
        private WindowModel CreateWindoModel(object viewModel)
        {
            var modelType = viewModel.GetType();
            var windowTypeName = modelType.Name.Replace("ViewModel", "View");
            var windowTypes = from t in modelType.Assembly.GetTypes()
                              where t.IsClass && t.Name == windowTypeName
                              select t;

            WindowModel model = GetWindowModelFromWindowName(windowTypes.Single(), viewModel);
            OpenedViews.Add(model);

            return model;
        }

        /// <summary>
        /// Creates instance of new window basing on window type, returns new instance of WindowModel object.
        /// </summary>
        /// <param name="type">Window type</param>
        /// <param name="viewModel">View model related to the window</param>
        /// <returns>WindowModel object</returns>
        private WindowModel GetWindowModelFromWindowName(Type type, object viewModel)
        {
            Window window = (Window)Activator.CreateInstance(type);
            window.DataContext = viewModel;

            WindowModel model = new WindowModel()
            {
                OpenedWindow = window,
                AssignedViewModel = viewModel,
                IsValueReturned = false,
                ReturnedDialogResult = null,
                ReturnedObjectResult = null,
                ReturnedFilePathResult = null
            };

            return model;
        }

        /// <summary>
        /// Verify if WindowModel can be found in OpenedViews.
        /// </summary>
        /// <param name="viewModel">View model related to the window</param>
        /// <returns>Bool type, if the window is opened already or not</returns>
        private bool CheckIfAlreadyOpened(object viewModel)
        {
            return OpenedViews.Select(model => model.AssignedViewModel.GetType() == viewModel.GetType()).FirstOrDefault();
        }

        /// <summary>
        /// Handler for Closed event, triggered when window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="model">Window model object related to the window</param>
        private void WindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => WindowClosed(s, e, model));

            OpenedViews.Remove(model);
        }

        /// <summary>
        /// Handler for modal dialog window Closed event, triggered when dialog window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="model">Window model object related to the window</param>
        private void DialogWindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => DialogWindowClosed(s, e, model));

            OpenedViews.Remove(model);

            var vm = window.DataContext as IModalDialogViewModel;
            model.ReturnedDialogResult = vm.DialogResult;
            model.IsValueReturned = true;
        }

        /// <summary>
        /// Handler  for modal dialog window Closed event, triggered when dialog window is about to close.
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="model">>Window model object related to the window</param>
        private void ResultWindowClosed(object sender, EventArgs args, WindowModel model)
        {
            Window window = (Window)sender;
            window.Closed -= new EventHandler((s, e) => ResultWindowClosed(s, e, model));

            OpenedViews.Remove(model);

            var vm = window.DataContext as IResultViewModel;
            model.ReturnedObjectResult = vm.ObjectResult;
            model.IsValueReturned = true;
        }

        /// <summary>
        /// Finds Window in OpenedViews List basing on VM object type, closes this window, removes from dictionary.
        /// </summary>
        /// <param name="viewModel">View model object</param>
        public void CloseWindow(object viewModel)
        {
            if (viewModel != null)
            {
                Type type = viewModel.GetType();
                Window window = OpenedViews.FirstOrDefault(model => model.AssignedViewModel.GetType() == type).OpenedWindow as Window;

                if (window != null)
                {
                    window.Close();
                }
            }
        }
    }
}
