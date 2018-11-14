using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ZERO.Material.Model
{
    public class Material_Company
    {
        public Material_Company()
        {
            this.Material_Product_Company = new HashSet<Material_Product_Company>();
        }

        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key]
        public string Company_Id { get; set; }

        public string Company_Name { get; set; }
        public string Company_Person { get; set; }
        public string Company_Person_Phone { get; set; }
        public string Company_Remark { get; set; }

        [JsonIgnore]
        public virtual ICollection<Material_Product_Company> Material_Product_Company { get; set; }
    }
}