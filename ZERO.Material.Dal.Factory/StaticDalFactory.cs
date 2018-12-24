﻿using ZERO.Material.Command;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal.Factory
{
    public class StaticDalFactory
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        public static IBaseDal GetIBaseDal() => Container.Server<IBaseDal>();

        public static IBaseCompanyDal GetIBaseCompanyDal() => Container.Server<IBaseCompanyDal>();

        public static IBaseInfoDal GetBaseInfoDal() => Container.Server<IBaseInfoDal>();

        public static IBaseTypeDal GetBaseTypeDal() => Container.Server<IBaseTypeDal>();

        public static ICompanyDal GetCompanyDal() => Container.Server<ICompanyDal>();

        public static ITypeDal GeTypeDal() => Container.Server<ITypeDal>();
    }
}