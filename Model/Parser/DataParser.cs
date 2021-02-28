using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using MVVM_SandBox.Model.Exceptions;

namespace MVVM_SandBox.Model.Parser
{
    /// <summary>
    /// Класс, занимающийся парсингом данных
    /// </summary>
    internal class DataParser
    {
        /// <summary>
        /// Порядок полей в целевом файле
        /// </summary>
        private List<string> _fieldOrder;
        /// <summary>
        /// Разделитель данных в целевом файле
        /// </summary>
        private char _separator;

        /// <summary>
        /// Конструктор по умолчанию
        /// Используется порядок полей как в задании
        /// </summary>
        /// <param name="separator">Разделитель данных в целевом файле</param>
        public DataParser(char separator = ';')
        {
            _separator = separator;
            _fieldOrder = new List<string>()
            {
                "Date",
                "ObjectA",
                "TypeA",
                "ObjectB",
                "TypeB",
                "Direction",
                "Color",
                "Intensity",
                "LatitudeA",
                "LongitudeA",
                "LatitudeB",
                "LongitudeB"
            };
        }
        /// <summary>
        /// Создает объект с пользовательским порядком полей
        /// </summary>
        /// <param name="firstDataString">Первая строка файла (название полей)</param>
        /// <param name="separator">Разделитель данных в целевом файле</param>
        public DataParser(string firstDataString, char separator = ';')
        {
            _separator = separator;
            var headers = firstDataString
                .Split(new char[] { _separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Replace(" ", ""));

            _fieldOrder = new List<string>(headers);
        }

        /// <summary>
        /// Асинхронно создает объект DataModel из строки данных 
        /// </summary>
        /// <param name="toParse">Строка с данными</param>
        /// <returns></returns>
        public async Task<DataModel> ParseOneStringAsync(string toParse)
        {
            var instance = new DataModel();
            await Task.Run(() =>
            {
                var data = toParse.Split(new char[] { _separator }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < data.Length; i++)
                {
                    var propName = _fieldOrder[i];
                    var type = typeof(DataModel);

                    try
                    {
                        var prop = type.GetProperty(propName);
                        prop.SetValue(instance, data[i]);
                    }
                    catch(ArgumentException)
                    {
                        try
                        {
                            var setter = type.GetProperty(propName + "Setter",
                            BindingFlags.NonPublic |
                            BindingFlags.Instance);

                            setter.SetValue(instance, data[i]);
                        }
                        catch
                        {
                            throw new ParseException(toParse);
                        }
                    }
                    
                }
            });
            return instance;
        }
    }
}
