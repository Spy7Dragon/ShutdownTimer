using System;

namespace ShutdownTimer
{
    /**
     *represents time 
     */
    public class Time
    {

        public Time(int theHour, int theMinute, string theToD)
        {
            m_hour = theHour;
            m_minute = theMinute;
            m_to_d = theToD;
        }

        public int GetHour()
        {
            return m_hour;
        }

        public int GetMinute()
        {
            return m_minute;
        }

        public string GetToD()
        {
            return m_to_d;
        }

        public override string ToString()
        {
            return this.GetHour().ToString("00") + ":" + this.GetMinute().ToString("00") + " " + this.GetToD();
        }

        private int m_hour;
        private int m_minute;
        private readonly string m_to_d;
    }
}
