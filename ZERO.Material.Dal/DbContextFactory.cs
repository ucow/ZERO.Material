using System.Data.Entity;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class DbContextFactory
    {
        private static ZERO_MaterialEntities _zeroMaterial = null;

        public static DbContext GetDbContext()
        {
            return _zeroMaterial ?? (_zeroMaterial = new ZERO_MaterialEntities());
        }
    }
}