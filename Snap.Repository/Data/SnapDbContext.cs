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

            modelBuilder.Entity<TripDay>()
                .HasOne(d => d.TripPlan)
                .WithMany(p => p.TripDays)
                .HasForeignKey(d => d.TripPlanId);

            modelBuilder.Entity<TripActivity>()
                .HasOne(a => a.TripDay)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.TripDayId);
        }


 // DbSet properties for each entity
        public DbSet<User> Users { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<TopPlace> TopPlaces { get; set; }
        public DbSet<Blogs> Blogs { get; set; }

        public DbSet<Preferences> Preferences { get; set; }

        public DbSet<TripPlan> TripPlans { get; set; }
        public DbSet<TripDay> TripDays { get; set; }
        public DbSet<TripActivity> TripActivities { get; set; }



    }
}
