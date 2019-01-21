using System.ComponentModel.DataAnnotations;
using ZERO.Material.Model.AttributeCustom;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(MaterialTypeValidate))]
    public partial class Material_Type
    {
    }

    public class MaterialTypeValidate
    {
        [Display(Name = "类别编号")]
        [IsSort]
        public string Material_Type_Id { get; set; }

        [Display(Name = "类别名称")]
        public string Material_Type_Name { get; set; }

        [Display(Name = "父类别")]
        public string Material_Type_Parent_Id { get; set; }

        [Display(Name = "备注")]
        public string Material_Type_Remark { get; set; }
    }
}