using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Base
    {
        public Material_Base()
        {
            this.Material_Base_Product = new HashSet<Material_Base_Product>();
            this.Material_Base_Type = new HashSet<Material_Base_Type>();
        }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key]
        [Display(Name = "器材编号")]
        public string Material_Id { get; set; }

        [Display(Name = "器材名称")]
        public string Material_Name { get; set; }

        [Display(Name = "器材总量")]
        public double Material_Count { get; set; }

        [Display(Name = "器材余量")]
        public double Material_Remain_Count { get; set; }

        [Display(Name = "器材单位")]
        public string Material_Count_Unit { get; set; }

        [Display(Name = "器材备注")]
        public string Material_Remark { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Base_Product> Material_Base_Product { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Base_Type> Material_Base_Type { get; set; }
    }
}