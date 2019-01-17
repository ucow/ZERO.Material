using System.ComponentModel.DataAnnotations;
using ZERO.Material.Model.AttributeCustom;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(MaterialCompanyValidate))]
    public partial class Material_Company
    {
    }

    public class MaterialCompanyValidate
    {
        [Display(Name = "公司编号")]
        [IsSort]
        public string Company_Id { get; set; }

        [Display(Name = "公司名称")]
        public string Company_Name { get; set; }

        [Display(Name = "备注")]
        public string Company_Remark { get; set; }
    }
}