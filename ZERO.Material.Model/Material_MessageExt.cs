using System.ComponentModel.DataAnnotations;
using System.Drawing;
using ZERO.Material.Model.AttributeCustom;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(MaterialMessageValidate))]
    public partial class Material_Info
    {
    }

    public class MaterialMessageValidate
    {
        [Display(Name = "器材编号")]
        public string Material_Id { get; set; }

        [Display(Name = "器材名称")]
        public string Material_Name { get; set; }

        [Display(Name = "供货商")]
        public string Company_Name { get; set; }

        [Display(Name = "器材类型")]
        public string Material_Type_Name { get; set; }

        [IsSort]
        [Display(Name = "器材总量")]
        public double Material_Count { get; set; }

        [IsSort]
        [Display(Name = "器材余量")]
        public double Material_RemainCont { get; set; }

        [Display(Name = "计量单位")]
        public string Material_CountUnit { get; set; }

        [IsSort]
        [Display(Name = "器材价格")]
        public double Material_Price { get; set; }

        [Display(Name = "单位重量")]
        public string Material_UnitWeight { get; set; }

        [Display(Name = "规格")]
        public string Material_Size { get; set; }

        [Display(Name = "备注")]
        public string Material_Remark { get; set; }
    }
}