

using Estate.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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