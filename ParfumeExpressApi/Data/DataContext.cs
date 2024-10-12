using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParfumeExpressApi.Models;

namespace ParfumeExpressApi.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        
        }
        public DbSet<Post> Posts { get; set; }
    }
}
