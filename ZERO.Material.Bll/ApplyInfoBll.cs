using System.Collections.Generic;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class ApplyInfoBll : BasicBll<Apply_Info>, IApplyInfoBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.ApplyInfoDal();
        }

        public override bool AddEntities(List<Apply_Info> ts)
        {
            List<Apply_Info> addApplyInfos = new List<Apply_Info>();
            List<Apply_Info> updateApplyInfos = new List<Apply_Info>();
            foreach (var item in ts)
            {
                Apply_Info applyInfo = GetEntity(m => m.Apply_Id == item.Apply_Id && m.Apply_Status == 0);
                if (applyInfo != null)
                {
                    applyInfo.Apply_Count += item.Apply_Count;
                    updateApplyInfos.Add(applyInfo);
                }
                else
                {
                    addApplyInfos.Add(item);
                }
            }

            return base.AddEntities(addApplyInfos) && base.UpdateEntities(updateApplyInfos);
        }

        //        public override bool UpdateEntities(List<Apply_Info> ts)
        //        {
        //            UnityContainerHelper container = new UnityContainerHelper();
        //            IBaseCompanyBll company = container.Server<IBaseCompanyBll>();
        //            List<Material_Base_Company> baseCompanies = new List<Material_Base_Company>();
        //
        //            foreach (var item in ts)
        //            {
        //                //使用申请
        //                if (item.ApplyType_Id == "001")
        //                {
        //                    string materialId = container.Server<IUseApplyBll>().GetEntity(m => m.Apply_Id == item.Apply_Id).Material_Id;
        //
        //                    Material_Base_Company baseCompany = company.GetEntity(m => m.Material_Id == materialId);
        //                    baseCompany.Material_RemainCont = baseCompany.Material_RemainCont - item.Apply_Count >= 0
        //                        ? baseCompany.Material_RemainCont - item.Apply_Count
        //                        : 0;
        //                    baseCompanies.Add(baseCompany);
        //                }
        //            }
        //
        //            return base.UpdateEntities(ts) && company.UpdateEntities(baseCompanies);
        //        }
    }
}