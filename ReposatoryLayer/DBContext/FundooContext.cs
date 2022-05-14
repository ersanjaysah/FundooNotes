using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.DBContext
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options) : base(options) { } //inherited base class
        public DbSet<User> Users { get; set; }
    }    
}
