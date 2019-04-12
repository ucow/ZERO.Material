using System.Data.Entity;
using System.Web;
using ZERO.Material.Model;

namespace ZERO.Material.Dal
{
    public class DbContextFactory
    {

        public static DbContext GetDbContext()
        {
            //            var context = HttpContext.Current.Items[nameof(DbContext)] as DbContext;
            //            if (context == null)
            //            {
            //                context = new ZERO_MaterialEntities1();
            //                HttpContext.Current.Items[nameof(DbContext)] = context;
            //            }
            //           
            //           return context as DbContext;
            return new ZERO_MaterialEntities1();


        }
    }
}