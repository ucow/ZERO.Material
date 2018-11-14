using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Type
    {
        public Material_Type()
        {
            this.Material_Base_Type = new HashSet<Material_Base_Type>();
        }

        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key]
        public string Material_Type_Id { get; set; }

        public string Material_Type_Name { get; set; }
        public string Material_Type_Remark { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Base_Type> Material_Base_Type { get; set; }
    }
}