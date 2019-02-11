using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class TeacherActionBll : BasicBll<Material_Teacher_Action>, ITeacherActionBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.TeacherActionDal();
        }
    }
}