using System.Data.Entity;

namespace ZERO.Material.Model
{
    public class ZeroMaterialContext : DbContext
    {
        public ZeroMaterialContext() : base("ZERO_Material")
        {
        }

        public DbSet<Material_Base> Material_Base { get; set; }
        public DbSet<Material_Base_Product> Material_Base_Product { get; set; }
        public DbSet<Material_Base_Type> Material_Base_Type { get; set; }
        public DbSet<Material_Company> Material_Company { get; set; }
        public DbSet<Material_Product> Material_Product { get; set; }
        public DbSet<Material_Product_Company> Material_Product_Company { get; set; }
        public DbSet<Material_Type> Material_Type { get; set; }
    }
}