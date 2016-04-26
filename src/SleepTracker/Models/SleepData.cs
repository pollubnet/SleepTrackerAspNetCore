#region License
/*
 * SleepTracker
 *
 * Written in 2016 by Marcin Badurowicz <m at badurowicz dot net>
 *
 * To the extent possible under law, the author(s) have dedicated
 * all copyright and related and neighboring rights to this 
 * software to the public domain worldwide. This software is 
 * distributed without any warranty. 
 *
 * You should have received a copy of the CC0 Public Domain 
 * Dedication along with this software. If not, see 
 * <http://creativecommons.org/publicdomain/zero/1.0/>. 
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepTracker.Models
{
    public class SleepData
    {
        public DateTime Alarm { get; set; }
        public int Snoozes { get; set; }
        public List<SleepMovements> Data { get; set; }
        public DateTime WokeUp { get; set; }

        public static SleepData Parse(string[] lines)
        {
            var result = new SleepData();
            result.Alarm = DateTime.Parse(lines[lines.Length - 4].Split(',')[0]);
            result.Snoozes = int.Parse(lines[lines.Length - 2].Split(',')[0]);

            result.Data = new List<SleepMovements>();

            for (int i = 0; i < lines.Length -5; i++)
            {
                var d = lines[i].Split(',');
                d[0] = d[0].Replace("<pre>", "");

                if (d[1].TrimEnd() == "-1")
                    break;

                var s = new SleepMovements { Time = d[0], Movements = int.Parse(d[1]) };
                result.Data.Add(s);
            }

            return result;
        }
    }

    public class SleepMovements
    {
        public string Time { get; set; }
        public int Movements { get; set; }
    }
}
