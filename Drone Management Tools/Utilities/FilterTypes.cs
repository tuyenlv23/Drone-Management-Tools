using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drone_Management_Tools.Utilities
{
    public static class FilterTypes
    {
        public const string ALL_FORMAT = "All Files (*.*)|*.*|Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Xml Files (*.xml)|*.xml|Json Files (*.json)|*.json";
        public const string EXCRL_FORMAT = "Excel Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv";
        public const string CSV_FORMAT = "Excel Files (*.csv)|*.csv";
        public const string XML_FORMAT = "Xml Files (*.xml)|*.xml";
        public const string JSON_FORMAT = "Json Files (*.json)|*.json";
        public const string IMAGE_FORMAT = "Image Files (*.jpg;*.jpeg;*.gif;*.bmp;*.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";
    }
}
