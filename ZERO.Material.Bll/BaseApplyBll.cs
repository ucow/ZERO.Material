using System.Collections.Generic;
using System.ComponentModel;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseApplyBll : BasicBll<Material_Apply>, IBaseApplyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BaseApplyDal();
        }

        public override bool AddEntities(List<Material_Apply> ts)
        {
            List<Material_Apply> applies = new List<Material_Apply>();
            UnityContainerHelper container = new UnityContainerHelper();
            foreach (var item in ts)
            {
                Material_Apply apply = GetEntity(m => string.Equals(m.Teacher_Id, item.Teacher_Id) &&
                                                      m.Apply_Time.Equals(item.Apply_Time) &&
                                                      m.Start_Time.Equals(item.Start_Time) &&
                                                      m.End_Time.Equals(item.End_Time) &&
                                                      string.Equals(m.Teach_Depart, item.Teach_Depart) &&
                                                      string.Equals(m.Teach_Field, item.Teach_Field) &&
                                                      string.Equals(m.Material_Id, item.Material_Id));
                if (apply != null)
                {
                    if (UnityContainerHelper.Server<IApplyInfoBll>().GetEntity(m => m.Apply_Id == apply.Id && m.Apply_Status == 0) != null)
                    {
                        item.Id = apply.Id;
                    }
                    else
                    {
                        applies.Add(item);
                    }
                }
                else
                {
                    applies.Add(item);
                }
            }

            return base.AddEntities(applies);
        }
    }
}