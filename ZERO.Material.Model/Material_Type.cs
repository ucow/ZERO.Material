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
    
    public partial class Material_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material_Type()
        {
            this.Material_Base = new HashSet<Material_Base>();
        }
    
        public string Material_Type_Id { get; set; }
        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }
        public string Material_Type_Name { get; set; }
        public string Material_Type_Remark { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material_Base> Material_Base { get; set; }
    }
}
