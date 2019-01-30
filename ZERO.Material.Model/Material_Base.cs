//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZERO.Material.Model
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    
    public partial class Material_Base
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material_Base()
        {
            this.BuyInComing_Apply = new HashSet<BuyInComing_Apply>();
            this.Material_Apply = new HashSet<Material_Apply>();
            this.Material_Base_Company = new HashSet<Material_Base_Company>();
            this.Material_Base_Type = new HashSet<Material_Base_Type>();
        }
    
        public string Material_Id { get; set; }
        public int Id { get; set; }
        public string Material_Name { get; set; }
        public string Material_Remark { get; set; }
        public byte[] Material_Image { get; set; }
    
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyInComing_Apply> BuyInComing_Apply { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Apply> Material_Apply { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Base_Company> Material_Base_Company { get; set; }
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Base_Type> Material_Base_Type { get; set; }
    }
}
