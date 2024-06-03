using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZotochkinaA_PR2_21._102.Model;

namespace ZotochkinaA_PR2_21._102
{
    public class greetUser
    {
        public static string GetTimeOfDayGreeting()
        {
            string timeOfDayGreeting = "";

            DateTime currentTime = DateTime.Now;
            if (currentTime.TimeOfDay >= new TimeSpan(10, 0, 0) && currentTime.TimeOfDay < new TimeSpan(12, 0, 0))
            {
                timeOfDayGreeting = "Доброе Утро";
            }
            else if (currentTime.TimeOfDay >= new TimeSpan(12, 0, 0) && currentTime.TimeOfDay < new TimeSpan(17, 0, 0))
            {
                timeOfDayGreeting = "Добрый День";
            }
            else if (currentTime.TimeOfDay >= new TimeSpan(17, 0, 0) && currentTime.TimeOfDay <= new TimeSpan(19, 0, 0))
            {
                timeOfDayGreeting = "Добрый Вечер";
            }

            return timeOfDayGreeting;
        }

    }
}
