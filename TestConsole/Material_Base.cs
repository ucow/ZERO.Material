//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestConsole
{
    using System;
    using System.Collections.Generic;
    
    public partial class Material_Base
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material_Base()
        {
            this.Material_Apply = new HashSet<Material_Apply>();
            this.Material_Base_Company = new HashSet<Material_Base_Company>();
            this.Material_Base_Type = new HashSet<Material_Base_Type>();
        }
    
        public string Material_Id { get; set; }
        public int Id { get; set; }
        public string Material_Name { get; set; }
        public string Material_Remark { get; set; }
        public byte[] Material_Image { get; set; }
        public bool Is_Show { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Apply> Material_Apply { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Base_Company> Material_Base_Company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Base_Type> Material_Base_Type { get; set; }
    }
}
