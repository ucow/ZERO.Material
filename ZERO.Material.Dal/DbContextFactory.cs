using System.Data.Entity;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class DbContextFactory
    {
        public static DbContext GetDbContext()
        {
            return new ZERO_MaterialEntities();
        }
    }
}