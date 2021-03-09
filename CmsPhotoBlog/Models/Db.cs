using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CmsPhotoBlog.Models;

namespace CmsPhotoBlog.Models
{
    public class Db : DbContext
    {
        public DbSet<Page> Pages { get; set; }
        public DbSet<Sidebar> Sidebars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogDetails> BlogDetailses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Db>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
