using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.DBContext
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options) : base(options)
        {

        }
         
        public DbSet<User> Users { get; set; }

        //method to used unique EmailId
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u=>u.Email)
                .IsUnique();
        }
    }    
}
