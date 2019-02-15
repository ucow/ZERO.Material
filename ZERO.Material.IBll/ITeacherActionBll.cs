using System.Collections.Generic;
using ZERO.Material.Model;

namespace ZERO.Material.IBll
{
    public interface ITeacherActionBll : IBasicBll<Material_Teacher_Action>
    {
        bool SetTeacherAction(string teacherId, List<int> roleActionIds, List<int> actionIds);
    }
}