using System.Collections.Generic;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;

namespace ZERO.Material.Bll
{
    public class UseApplyBll : BasicBll<Use_Apply>, IUseApplyBll
    {

        public override void SetBasicDal()
        {
            BasicDal = DbSession.UseApplyDal();
        }


    }
}