using System.Collections.Generic;
using System.ComponentModel;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BuyInComingApplyBll : BasicBll<BuyInComing_Apply>, IBuyInComingApplyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BuyInComingApplyDal();
        }

        public override bool AddEntities(List<BuyInComing_Apply> ts)
        {
            List<BuyInComing_Apply> addApplies = new List<BuyInComing_Apply>();
            foreach (BuyInComing_Apply buyInComingApply in ts)
            {
                BuyInComing_Apply buyIn = GetEntity(m => m.Material_Id == buyInComingApply.Material_Id);
                if (buyIn != null)
                {
                    if (UnityContainerHelper.Server<IApplyInfoBll>().GetEntity(m => m.Apply_Id == buyIn.Id && m.Apply_Status == 0) != null)
                    {
                        buyInComingApply.Id = buyIn.Id;
                    }
                    else
                    {
                        addApplies.Add(buyInComingApply);
                    }
                }
                else
                {
                    addApplies.Add(buyInComingApply);
                }
            }

            return base.AddEntities(addApplies);
        }
    }
}