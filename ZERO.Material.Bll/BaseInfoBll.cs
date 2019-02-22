using System.Collections.Generic;
using System.ComponentModel;
using ZERO.Material.Command;
using ZERO.Material.Dal.Factory;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseInfoBll : BasicBll<Material_Info>, IBaseInfoBll
    {
        public BaseInfoBll()
        {
        }

        public override void SetBasicDal()
        {
            BasicDal = DbSession.BaseInfoDal();
        }

        public string UpdateBaseInfo(Material_Info materialInfo, string oldTypeName, string oldCompanyName)
        {
            //更新Material_Base表
            IBaseBll baseBll = UnityContainerHelper.Server<IBaseBll>();
            Material_Base materialBase = baseBll.Find(materialInfo.Material_Id);
            if (materialBase != null)
            {
                materialBase.Material_Name = materialInfo.Material_Name;
                materialBase.Material_Image = materialInfo.Material_Image;
                materialBase.Material_Remark = materialInfo.Material_Remark;
            }

            baseBll.UpdateEntities(new List<Material_Base>()
            {
                materialBase
            });
            //更新Material_Base_Type表
            IBaseTypeBll baseTypeBll = UnityContainerHelper.Server<IBaseTypeBll>();
            ITypeBll typeBll = UnityContainerHelper.Server<ITypeBll>();
            Material_Base_Type baseType = baseTypeBll.Find(typeBll.GetEntity(m => m.Material_Type_Name == oldTypeName).Material_Type_Id, materialInfo.Material_Id);
            if (baseType != null)
            {
                baseTypeBll.DeleteEntity(new List<Material_Base_Type> { baseType });
            }
            baseTypeBll.AddEntities(new List<Material_Base_Type>
            {
                new Material_Base_Type
                {
                    Material_Id = materialInfo.Material_Id,
                    Material_Type_Id = typeBll.GetEntity(m => m.Material_Type_Name == materialInfo.Material_Type_Name)
                        .Material_Type_Id
                }
            });

            //更新Material_Base_Company表
            IBaseCompanyBll baseCompanyBll = UnityContainerHelper.Server<IBaseCompanyBll>();
            ICompanyBll companyBll = UnityContainerHelper.Server<ICompanyBll>();
            Material_Base_Company baseCompany = baseCompanyBll.Find(companyBll.GetEntity(m => m.Company_Name == oldCompanyName).Company_Id,
                materialInfo.Material_Id);
            if (baseCompany != null)
            {
                baseCompanyBll.DeleteEntity(new List<Material_Base_Company>() { baseCompany });
            }
            baseCompanyBll.AddEntities(new List<Material_Base_Company>
            {
                new Material_Base_Company
                {
                    Material_Id = materialInfo.Material_Id,
                    Company_Id = companyBll.GetEntity(m => m.Company_Name == materialInfo.Company_Name).Company_Id,
                    Material_Count = materialInfo.Material_Count,
                    Material_CountUnit = materialInfo.Material_CountUnit,
                    Material_Size =  materialInfo.Material_Size,
                    Material_UnitWeight = materialInfo.Material_UnitWeight,
                    Material_Price = materialInfo.Material_Price,
                    Material_RemainCont = materialInfo.Material_RemainCont,
                }
            });

            DbSession.SaveChanges();
            return "修改成功";
        }

        public string AddBaseInfo(Material_Info materialInfo)
        {
            UnityContainerHelper UnityContainerHelper = new UnityContainerHelper();
            IBaseBll baseBll = UnityContainerHelper.Server<IBaseBll>();
            ITypeBll typeBll = UnityContainerHelper.Server<ITypeBll>();
            ICompanyBll companyBll = UnityContainerHelper.Server<ICompanyBll>();
            IBaseTypeBll baseTypeBll = UnityContainerHelper.Server<IBaseTypeBll>();
            IBaseCompanyBll baseCompanyBll = UnityContainerHelper.Server<IBaseCompanyBll>();

            bool baseIsNull = true;
            bool typeIsNull = true;
            bool companyIsNull = true;

            if (baseBll.Find(materialInfo.Material_Id) == null)
            {
                baseIsNull = false;
                baseBll.AddEntities(new List<Material_Base>()
                {
                    new Material_Base()
                    {
                        Material_Id = materialInfo.Material_Id,
                        Material_Name = materialInfo.Material_Name,
                        Material_Image = materialInfo.Material_Image,
                        Material_Remark = materialInfo.Material_Remark
                    }
                });
            }

            if (baseCompanyBll.Find(companyBll.GetEntity(m => m.Company_Name == materialInfo.Company_Name).Company_Id,
                    materialInfo.Material_Id) == null)
            {
                companyIsNull = false;
                baseCompanyBll.AddEntities(new List<Material_Base_Company>()
                {
                    new Material_Base_Company()
                    {
                        Material_Id = materialInfo.Material_Id,
                        Company_Id = companyBll.GetEntity(m => m.Company_Name == materialInfo.Company_Name).Company_Id,
                        Material_Count = materialInfo.Material_Count,
                        Material_CountUnit = materialInfo.Material_CountUnit,
                        Material_Size =  materialInfo.Material_Size,
                        Material_UnitWeight = materialInfo.Material_Size,
                        Material_Price = materialInfo.Material_Price,
                        Material_RemainCont = materialInfo.Material_RemainCont,
                    }
                });
            }

            if (baseTypeBll.Find(typeBll.GetEntity(m => m.Material_Type_Name == materialInfo.Material_Type_Name).Material_Type_Id, materialInfo.Material_Id) == null)
            {
                typeIsNull = false;
                baseTypeBll.AddEntities(new List<Material_Base_Type>()
                {
                    new Material_Base_Type()
                    {
                        Material_Id = materialInfo.Material_Id,
                        Material_Type_Id = typeBll.GetEntity(m => m.Material_Type_Name == materialInfo.Material_Type_Name)
                            .Material_Type_Id
                    }
                });
            }

            if (baseIsNull && companyIsNull && typeIsNull)
            {
                return "该数据已存在，无法添加";
            }

            DbSession.SaveChanges();
            return "添加成功";
        }
    }
}