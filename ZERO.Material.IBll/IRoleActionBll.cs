using System.Collections.Generic;
using ZERO.Material.Model;

namespace ZERO.Material.IBll
{
    public interface IRoleActionBll : IBasicBll<Material_Role_Action>
    {
        bool SetRoleByAction(List<int> roleIds, int actionId);

        bool SetActionByRole(List<int> actionIds, int roleId);
    }
}