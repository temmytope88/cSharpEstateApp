using Estate.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Estateapp.Data.DBContext.AuthenticationDBContext
{
  public class AuthenticationDBContext: IdentityDbContext<ApplicationUser> 
  {
    public AuthenticationDBContext(DbContextOptions<AuthenticationDBContext> options)
       :base(options)
    {
      
    }
  }
}