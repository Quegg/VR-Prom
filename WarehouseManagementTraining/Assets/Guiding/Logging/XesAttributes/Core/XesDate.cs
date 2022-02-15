using System;

namespace XesAttributes
{
    public class XesDate:XesAttribute
    {
        private DateTime date;

        public XesDate(DateTime dateTime)
        {
            this.date = dateTime;
        }


        public XesDate(int year, int month, int day, int hour, int minute, int second, int milliSeconds)
        {
            date = new DateTime(year,month,day,hour,minute,second,milliSeconds);
        }

        public override string ToString()
        {
            //Extract Local Timezone Information for the format xes needs
            string timeZone = TimeZoneInfo.Local.DisplayName.Split('C')[1].Split(')')[0];
            return date.Year + "-" + date.Month.ToString("D2") + "-" + date.Day.ToString("D2") + "T" + date.Hour.ToString("D2") + ":" + date.Minute.ToString("D2") + ":" + date.Second.ToString("D2") +
                   "." + date.Millisecond.ToString("D3")+timeZone;
        }
            
            
    }
}
