using System;
using System.Globalization;

namespace MVVM_SandBox.Model
{
    /// <summary>
    /// Модель данных, описанных в задании
    /// Прим. приватные свойства используются для задания значения публичным через рефлексию (см. DataParser)
    /// </summary>
    internal class DataModel
    {
        private string DateSetter
        {
            set => Date = DateTime.Parse(value);
        }
        public DateTime Date { get; set; }
        public string ObjectA { get; set; }
        public string TypeA { get; set; }
        public string ObjectB { get; set; }
        public string TypeB { get; set; }
        public string Direction { get; set; }
        public string Color { get; set; }

        
        private string IntensitySetter
        {
            set => Intensity = int.Parse(value);
        }
        public int Intensity { get; set; }

        
        private string LatitudeASetter
        {
            set => LatitudeA = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        public double LatitudeA { get; set; }

        private string LongitudeASetter
        {
            set => LongitudeA = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        public double LongitudeA { get; set; }

        private string LatitudeBSetter
        {
            set => LatitudeB = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        public double LatitudeB { get; set; }

        private string LongitudeBSetter
        {
            set => LongitudeB = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }
        public double LongitudeB { get; set; }
    }
}
