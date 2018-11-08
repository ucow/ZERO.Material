using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZERO.Material.Model
{
    public class Material_Base_Type
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }

        [Key, Column(Order = 0)]
        public string Material_Id { get; set; }

        [Key, Column(Order = 1)]
        public string Type_Id { get; set; }

        public virtual Material_Base Material_Base { get; set; }
        public virtual Material_Type Material_Type { get; set; }
    }
}