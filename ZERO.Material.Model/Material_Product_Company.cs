//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZERO.Material.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Material_Product_Company
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }
        public string Product_Id { get; set; }
        public string Company_Id { get; set; }
    
        public virtual Material_Company Material_Company { get; set; }
        public virtual Material_Product Material_Product { get; set; }
    }
}
