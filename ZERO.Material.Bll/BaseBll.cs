using System.Collections.Generic;
using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseBll : BasicBll<Material_Base>, IBaseBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IBaseDal>();
        }

        public override bool AddOrUpdateEntity(List<Material_Base> ts)
        {
            List<Material_Base> addOrUpdates = new List<Material_Base>();
            foreach (Material_Base materialBase in ts)
            {
                Material_Base material = BasicDal.GetEntity(m => m.Material_Id == materialBase.Material_Id);
                if (material != null)
                {
                    materialBase.Id = material.Id;
                }
                addOrUpdates.Add(materialBase);
            }

            return BasicDal.AddOrUpdateEntity(addOrUpdates);
        }
    }
}