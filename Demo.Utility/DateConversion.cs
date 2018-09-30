using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Utility
{
  public  class DateConversion
    {
        public static DateTime UTCDateAndTime(DateTime dt, string standardTimeZone, bool isConvertFromUTC)
        {
            DateTime returnDateTime = DateTime.MinValue;




            TimeZoneInfo tzi;
            try
            {
                tzi = TimeZoneInfo.FindSystemTimeZoneById(standardTimeZone);
            }
            catch (Exception ex)
            {
                tzi = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }

            if (isConvertFromUTC)
                returnDateTime = TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
            else
                returnDateTime = TimeZoneInfo.ConvertTimeToUtc(dt, tzi);

            return returnDateTime;
        }
    
    }
}
