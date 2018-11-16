using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Base_Product
    {
        [JsonIgnore]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key, Column(Order = 0)]
        public string Material_Id { get; set; }

        [Key, Column(Order = 1)]
        public string Product_Id { get; set; }

        [JsonIgnore]
        public virtual Material_Base Material_Base { get; set; }

        [JsonIgnore]
        public virtual Material_Product Material_Product { get; set; }
    }
}