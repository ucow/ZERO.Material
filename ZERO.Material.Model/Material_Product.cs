using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Product
    {
        public Material_Product()
        {
            this.Material_Base_Product = new HashSet<Material_Base_Product>();
            this.Material_Product_Company = new HashSet<Material_Product_Company>();
        }

        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key]
        public string Product_Id { get; set; }

        public string Product_Format { get; set; }
        public Nullable<double> Product_Price { get; set; }
        public string Product_Remark { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Base_Product> Material_Base_Product { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Product_Company> Material_Product_Company { get; set; }
    }
}