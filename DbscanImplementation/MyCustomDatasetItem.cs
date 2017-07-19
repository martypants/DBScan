
using System.Data;
using System; 

namespace DbscanImplementation
{
    public class MyCustomDatasetItem : DatasetItemBase
    {
        public double X;
        public double Y;

        public DateTime _logTime; 



        public MyCustomDatasetItem(double x, double y)
        {
            X = x;
            Y = y;
        }

        public MyCustomDatasetItem(double x, double y , DateTime lt)
        {
            X = x;
            Y = y;
            _logTime = lt; 
        }

    }
}