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
    using System;using Newtonsoft.Json;
    using System.Collections.Generic;
    
    public partial class Material_Action
    {
        public int Id { get; set; }
        public string Action_Name { get; set; }
        public string Action_Url { get; set; }
        public string Http_Method { get; set; }
        public int Menu_Id { get; set; }
        public bool Is_Menu { get; set; }
        public bool Del_Flag { get; set; }
        public string Action_Remark { get; set; }
    }
}
