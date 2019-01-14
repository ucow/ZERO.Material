using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZERO.Material.Model
{
    [MetadataType(typeof(TeacherExt))]
    public partial class Material_Teacher
    {
    }

    public class TeacherExt
    {
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(maximumLength: 4, MinimumLength = 4, ErrorMessage = "教师编号必须4位")]
        [DisplayName("教师编号")]
        public string Teacher_Id { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("教师姓名")]
        public string Teacher_Name { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("教师系别")]
        public string Teacher_Depart { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [DisplayName("教师密码")]
        public string Teacher_Password { get; set; }
    }
}