using ZERO.Material.Model;

namespace ZERO.Material.IBll
{
    public interface IBaseInfoBll : IBasicBll<Material_Info>
    {
        string UpdateBaseInfo(Material_Info materialInfo, string oldTypeName, string oldCompanyName);

        string AddBaseInfo(Material_Info materialInfo);
    }
}