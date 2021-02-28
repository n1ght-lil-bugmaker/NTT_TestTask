using Microsoft.Win32;
using MVVM_SandBox.Commands;
using MVVM_SandBox.Model;
using MVVM_SandBox.Model.Loader;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace MVVM_SandBox.ViewModel
{
    /// <summary>
    /// Модель отображения главного окна
    /// </summary>
    internal class MainWindowViewModel : ViewModel
    {  
        /// <summary>
        /// Коллекция объектов DataModel, подгруженных в конкретный момент
        /// </summary>
        public ObservableCollection<DataModel> DataCollection { get; } = new ObservableCollection<DataModel>();

        private Loader _loader = new Loader();

        #region Properties

            #region Progress

        /// <summary>
        /// Прогресс загрузки
        /// </summary>
        private int _progress = 0;
        public int Progress
        {
            get => _progress;
            set => Set(ref _progress, value);
        }
        #endregion

            #region LoadStatus 

        /// <summary>
        /// Статус загрузки
        /// </summary>
        private string _loadStatus = "Ready";
        public string LoadStatus
        {
            get => _loadStatus;
            set => Set(ref _loadStatus, value);
        }
        #endregion

            #region MaxProgress

        /// <summary>
        /// Максимально возможный прогресс (используется для полосы загрузки)
        /// </summary>
        private int _maxProgress = 1;
        public int MaxProgress
        {
            get => _maxProgress;
            set => Set(ref _maxProgress, value);
        }
        #endregion

            #region Path
        /// <summary>
        /// Путь до целевого файла
        /// </summary>

        private string _path = "Путь не указан";
        public string Path
        {
            get => _path;
            set => Set(ref _path, value);
        }

            #endregion

        #endregion
        #region Commands

            #region LoadCommand

        /// <summary>
        /// Команда загрузки
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Проверяет возможность выполнения команды загрузки
        /// </summary>
        /// <param name="parameter">Параметр проверки</param>
        /// <returns>Возможность выполнения команды</returns>
        private bool CanLoadCommandExecuted(object parameter) => _loader.Status == Model.Loader.LoadStatus.Completed && _path != "Путь не указан";

        /// <summary>
        /// Выполняет команду звгрузки файла
        /// </summary>
        /// <param name="parameter">Параметр загрузки</param>
        private async void LoadCommandExecute(object parameter)
        {
            try
            {
                DataCollection.Clear();
                await _loader.LoadAsync(_path);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            #endregion

            #region InterruptLoadingCommand

        /// <summary>
        /// Команда прерывания загрузки
        /// </summary>
        public ICommand InteruptLoadingCommand { get; }


        /// <summary>
        /// Проверяет возможность прерывания загрузки
        /// </summary>
        /// <param name="parameter">Параметр проверки</param>
        /// <returns>Можно ли выполнить команду</returns>
        private bool CanInteruptLoadingCommandExecuted(object parameter) => _loader.Status == Model.Loader.LoadStatus.Loading;

        /// <summary>
        /// Выполняет команду прерывания загрузки
        /// </summary>
        /// <param name="parameter">Параметр выполнения</param>
        private void ExecuteInteruptLoadingCommand(object parameter) => _loader.Interrupt();

        #endregion

            #region BrowseFileCommand


        /// <summary>
        /// Команда выбора файла
        /// </summary>
        public ICommand BrowseFileCommand { get; }


        /// <summary>
        /// Проверяет возможность выбора файла
        /// </summary>
        /// <param name="parameter">Параметр проверки</param>
        /// <returns></returns>

        public bool CanBrowseFileCommandExecuted(object parameter) => true;


        /// <summary>
        /// Выполняет команду выбора файла
        /// </summary>
        /// <param name="parameter">Параметр выполнения</param>
        public void ExecuteBrowseFileCommand(object parameter)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if(dialog.ShowDialog() == true)
            {
                Path = dialog.FileName;
            }
        }
            #endregion

        #endregion

        /// <summary>
        /// Создает объект модели отображения главного окна
        /// </summary>
        public MainWindowViewModel()
        {
            LoadCommand = new LambdaCommand(CanLoadCommandExecuted, LoadCommandExecute);
            InteruptLoadingCommand = new LambdaCommand(CanInteruptLoadingCommandExecuted, ExecuteInteruptLoadingCommand);
            BrowseFileCommand = new LambdaCommand(CanBrowseFileCommandExecuted, ExecuteBrowseFileCommand);

            _loader.PropertyChanged += (sender, args) =>
            {
                Progress = _loader.Progress;
                MaxProgress = _loader.TotalLines;
                LoadStatus = _loader.Status.ToString();

                if(_loader.LoadedData != null)
                {
                    if (DataCollection.Count == 0)
                    {
                        DataCollection.Add(_loader.LoadedData);
                    }
                    else if (DataCollection[DataCollection.Count - 1] != _loader.LoadedData)
                    {
                        DataCollection.Add(_loader.LoadedData);
                    }
                }
                
            };
        }
    }
}
