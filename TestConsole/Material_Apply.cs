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
    
    public partial class Material_Apply
    {
        public int Id { get; set; }
        public string Teacher_Id { get; set; }
        public System.DateTime Apply_Time { get; set; }
        public System.DateTime Start_Time { get; set; }
        public System.DateTime End_Time { get; set; }
        public string Teach_Depart { get; set; }
        public string Teach_Field { get; set; }
        public string Material_Id { get; set; }
        public bool Is_Get { get; set; }
    
        public virtual Material_Base Material_Base { get; set; }
        public virtual Material_Teacher Material_Teacher { get; set; }
    }
}