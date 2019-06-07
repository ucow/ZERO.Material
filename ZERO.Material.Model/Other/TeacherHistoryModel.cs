using System;
using System.Collections.Generic;

namespace ZERO.Material.Model.Other
{
    public class TeacherHistoryModel
    {
        public string MaterialName { get; set; }
        public List<NameToCount> NameToCounts{ get; set; }
    }

    public class NameToCount
    {
        public DateTime Apply_Time { get; set; }
        public int Apply_Count { get; set; }
    }
}