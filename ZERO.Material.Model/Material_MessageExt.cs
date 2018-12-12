using System.ComponentModel.DataAnnotations;
using ZERO.Material.Model.AttributeCustom;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(MaterialMessageValidate))]
    public partial class Material_Message
    {
    }

    public class MaterialMessageValidate
    {
        [Display(Name = "器材编号")]
        public string Material_Id { get; set; }

        [Display(Name = "器材名称")]
        public string Material_Name { get; set; }

        [Display(Name = "器材类型")]
        public string Material_Type_Name { get; set; }

        [IsSort]
        [Display(Name = "器材总量")]
        public double Material_Count { get; set; }

        [IsSort]
        [Display(Name = "器材余量")]
        public double Material_Remain_Count { get; set; }

        [Display(Name = "计量单位")]
        public string Material_Count_Unit { get; set; }

        [Display(Name = "供货商")]
        public string Company_Name { get; set; }

        [IsSort]
        [Display(Name = "器材价格")]
        public double Material_Price { get; set; }

        [Display(Name = "备注")]
        public string Material_Remark { get; set; }
    }
}