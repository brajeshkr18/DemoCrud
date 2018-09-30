using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Utility
{
  public  class TypeConverter
    {
        public static string ConverToStringRoundOff(string Number)
        {
            return Math.Round(Convert.ToDouble(Number), 2).ToString(); ;
        }
        public static float ConverNumberToRoundOff(float? Number)
        {
            return (float)Math.Round((Decimal)Number, 2, MidpointRounding.AwayFromZero);
            //return Math.Round((float?)(Number), 2) ;
        }
        public static float ConverToNullStringToFloat(float? Number)
        {
           return (float)(Number);
        }
        public static float ConverStringToFloat(string Number)
        {
            return float.Parse(Number);
        }
        public static float? ToSingle(double ?value)
        {
            return (float?)value;
        }
        public static Nullable<double> convertdec(Nullable<double> val)
        {
            if (val.HasValue)
                return Math.Round(val.Value, 2);
            return val;
        }
       
    }
}
