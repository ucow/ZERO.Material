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
    
    public partial class Material_Base_Company
    {
        public int Id { get; set; }
        public string Company_Id { get; set; }
        public string Material_Id { get; set; }
        public double Material_Price { get; set; }
        public double Material_Count { get; set; }
        public string Material_Size { get; set; }
        public double Material_RemainCont { get; set; }
        public string Material_CountUnit { get; set; }
        public string Material_UnitWeight { get; set; }
    
        public virtual Material_Base Material_Base { get; set; }
        public virtual Material_Company Material_Company { get; set; }
    }
}
