using System;
using System.ComponentModel.DataAnnotations;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(Material_ApplyExt))]
    public partial class Material_Apply : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool Equals(Material_Apply other)
        {
            return string.Equals(Teacher_Id, other.Teacher_Id) &&
                   Apply_Time.Equals(other.Apply_Time) &&
                   Start_Time.Equals(other.Start_Time) &&
                   End_Time.Equals(other.End_Time) &&
                   string.Equals(Teach_Depart, other.Teach_Depart) &&
                   string.Equals(Teach_Field, other.Teach_Field) &&
                   string.Equals(Material_Id, other.Material_Id);
        }
    }

    public class Material_ApplyExt
    {
        [Display(Name = "讲师编号")]
        public string Teacher_Id { get; set; }

        [Display(Name = "申请时间")]
        public Nullable<System.DateTime> Apply_Time { get; set; }

        [Display(Name = "使用时间")]
        public Nullable<System.DateTime> Start_Time { get; set; }

        [Display(Name = "归还时间")]
        public Nullable<System.DateTime> End_Time { get; set; }

        [Display(Name = "使用班级系别")]
        public string Teach_Depart { get; set; }

        [Display(Name = "使用班级专业")]
        public string Teach_Field { get; set; }

        [Display(Name = "申请器材编号")]
        public string Material_Id { get; set; }
    }
}