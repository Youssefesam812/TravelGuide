using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snap.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Repository.Data
{
    public class SnapDbContext: IdentityDbContext<User>
    {

        public SnapDbContext(DbContextOptions<SnapDbContext>options) : base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);

        }


 // DbSet properties for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<TopPlace> TopPlaces { get; set; }
        public DbSet<Blogs> Blogs { get; set; }




    }
}
