using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;

namespace RentApp.Infrastructure
{
  public class Tenant : Person
  {
    public Flat Flat { get; set; }
  }
}
