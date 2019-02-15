using System.Collections.Generic;
using ZERO.Material.Model;

namespace ZERO.Material.IBll
{
    public interface IRoleTeacherBll : IBasicBll<Material_Role_Teacher>
    {
        bool SetTeacherRole(string teacherId, List<int> roleIds);
    }
}