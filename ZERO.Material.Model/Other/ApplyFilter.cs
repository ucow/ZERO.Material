using System;
using System.Text.RegularExpressions;

namespace ZERO.Material.Model.Other
{
    public class ApplyFilter
    {
        public string Teacher { get; set; }
        public string ApplyTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Status { get; set; }

        public DateTime[] GetStartAndEndTime(string times)
        {
            if (times == null)
            {
                return null;
            }

            string[] timeStrings = Regex.Split(times, " - ", RegexOptions.IgnoreCase);
            if (timeStrings.Length == 0)
            {
                return null;
            }

            if (DateTime.TryParse(timeStrings[0], out DateTime start) && DateTime.TryParse(timeStrings[1], out DateTime end))
            {
                return new DateTime[]
                {
                    start,end
                };
            }
            else
            {
                return null;
            }
        }
    }
}