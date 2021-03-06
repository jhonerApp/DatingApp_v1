using DatinApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatinApp.API.Data {
    public class DataContext : DbContext {
        public DataContext (DbContextOptions<DataContext> options) : base (options) { }
        public DbSet<tbl_values> tbl_values { get; set; }
        public DbSet<tbl_user> tbl_user { get; set; }
    
    }
}