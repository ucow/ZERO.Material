using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Product_Company
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key, Column(Order = 0)]
        public string Product_Id { get; set; }

        [Key, Column(Order = 1)]
        public string Company_Id { get; set; }

        [JsonIgnore]
        public virtual Material_Company Material_Company { get; set; }

        [JsonIgnore]
        public virtual Material_Product Material_Product { get; set; }
    }
}