using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class TeacherBll : BasicBll<Material_Teacher>, ITeacherBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.TeacherDal();
        }
    }
}