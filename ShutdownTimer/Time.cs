using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutdownTimer
{
    /**
     *represents time 
     */
    public class Time
    {

        public Time(int theHour, int theMinute, string theToD)
        {
            hour = theHour;
            minute = theMinute;
            ToD = theToD;
        }

        public int getHour()
        {
            return hour;
        }

        public int getMinute()
        {
            return minute;
        }

        public String getToD()
        {
            return ToD;
        }

        public String ToString()
        {
            return this.getHour().ToString("00") + ":" + this.getMinute().ToString("00") + " " + this.getToD();
        }

        private int hour;
        private int minute;
        private String ToD;
    }
}
