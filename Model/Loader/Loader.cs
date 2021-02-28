using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using MVVM_SandBox.Model.Exceptions;
using MVVM_SandBox.Model.Parser;

namespace MVVM_SandBox.Model.Loader
{
    /// <summary>
    /// Класс, подгружающий некоторые данные
    /// </summary>
    internal class Loader : INotifyPropertyChanged
    {
        /// <summary>
        /// Статус загрузки
        /// </summary>
        public LoadStatus Status { get; private set; } = LoadStatus.Completed;

        /// <summary>
        /// Данные, загруженные в конкретный момент
        /// </summary>
        public DataModel LoadedData { get; private set; }

        /// <summary>
        /// Прогресс загрузки
        /// </summary>
        public int Progress { get; private set; }

        /// <summary>
        /// Количество строк в текущем файле
        /// </summary>
        public int TotalLines { get; private set; }

        
        /// <summary>
        /// Событие изменения каких-либо свойств 
        /// </summary>

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызывает срабатывание события PropertyChanged
        /// </summary>
        /// <param name="sender">Вызывающий объект</param>
        /// <param name="args">Аргументы события</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(sender.ToString()));
        }

        /// <summary>
        /// Описывает, был ли прерван процесс загрузки
        /// </summary>
        private bool _interrupted = false;

        /// <summary>
        /// Асинхронно загружает информацию из файла, изменяя свойство LoadedData
        /// </summary>
        /// <param name="path">Путь до файла</param>
        /// <returns></returns>
        public async Task LoadAsync(string path)
        {
            DataParser parser = null;
            Progress = 0;
            LoadedData = null;
            await CountLinesAsync(path);
            OnPropertyChanged("Progress", new PropertyChangedEventArgs("Progress"));
            
            try
            {
                using(var fs = new FileStream(path, FileMode.Open))
                {
                    Status = LoadStatus.Loading;
                    _interrupted = false;

                    OnPropertyChanged("Progress", new PropertyChangedEventArgs("Progress"));

                    using(var reader = new StreamReader(fs))
                    {
                        string line = "";
                        bool isFirstLine = true;
                        for(int i = 0; i < TotalLines; i++)
                        {
                            if(_interrupted)
                            {
                                break;
                            }
                            line = await reader.ReadLineAsync();

                            if(isFirstLine)
                            {
                                parser = new DataParser(line);
                                isFirstLine = false;
                                continue;
                            }

                            if (line != "")
                            {
                                LoadedData = await parser.ParseOneStringAsync(line);
                                Progress++;
                                OnPropertyChanged(nameof(LoadedData), new PropertyChangedEventArgs(nameof(LoadedData)));
                                //await Task.Delay(100); 
                                //Показать, что программа не подвисает при загрузке
                            }
                        }
                        if (!_interrupted)
                        {
                            Progress = TotalLines;
                            Status = LoadStatus.Completed;
                        }
                    }
                }
            }
            catch(ParseException ex)
            {
                throw ex;
            }
            catch
            {
                throw new LoadException();
            }
            finally
            {
                Status = LoadStatus.Completed;
                OnPropertyChanged(nameof(Progress), new PropertyChangedEventArgs(nameof(Progress)));
            }
            
        }
        
        /// <summary>
        /// Прерывает процесс загрузки
        /// </summary>
        public void Interrupt()
        {
            _interrupted = true;
            Status = LoadStatus.Completed;
        }

        /// <summary>
        /// Асинхронно считает количество строк в файле.
        /// Streams используется для быстродействия, файл может быть большой, поэтому File.ReadAllLines().Length будет подтормаживать
        /// </summary>
        /// <param name="path">Путь до файла</param>
        /// <returns></returns>
        private async Task CountLinesAsync(string path)
        {
            int res = 0;

            using (var fs = new FileStream(path, FileMode.Open))
            {
                using(var reader = new StreamReader(fs))
                {
                    while(await reader.ReadLineAsync() != null)
                    {
                        res++;
                    }
                    TotalLines = res;
                    OnPropertyChanged(nameof(TotalLines), new PropertyChangedEventArgs(nameof(TotalLines)));
                }
            }
        }
    }
}
