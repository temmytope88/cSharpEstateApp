using Estateapp.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estateapp.Data.DBContext.ApplicationDBContext
{
  public class ApplicationDBContext : DbContext
  {
      public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) :base(options)
      {

      }
      public DbSet<Property> properties {get; set;}
      public DbSet<Contact> contacts {get; set;}
  }
}