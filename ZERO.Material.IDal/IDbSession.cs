namespace ZERO.Material.IDal
{
    public interface IDbSession
    {
        IBaseDal BaseDal();

        IBaseCompanyDal BaseCompanyDal();

        IBaseInfoDal BaseInfoDal();

        IBaseTypeDal BaseTypeDal();

        ICompanyDal CompanyDal();

        ITypeDal TypeDal();

        int SaveChanges();
    }
}