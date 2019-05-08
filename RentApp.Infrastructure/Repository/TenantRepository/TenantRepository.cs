using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository.TenantRepository
{
  public class TenantRepository : ITenantRepository
  {
    private readonly RentContext _rentContext;

    public TenantRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }
    
    public async Task<IEnumerable<Tenant>> GetAll()  
    {
      var tenants = await _rentContext.Tenant.ToListAsync();
      tenants.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Flat).LoadAsync();});
    
      return tenants;
    }

   
    public async Task<Tenant> GetById(long id)
    {
      var tenant = await _rentContext.Tenant
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();
      await _rentContext.Entry(tenant).Reference(x => x.Flat).LoadAsync();
     
      return tenant;
    }

    public async Task Add(Tenant tenant)
    {
      tenant.DateOfCreation = DateTime.Now;
      await _rentContext.Tenant
        .Include(x => x.Flat)
        .FirstAsync();
      await _rentContext.Tenant.AddAsync(tenant);
      await _rentContext.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
      var tenantToDelete = await _rentContext.Tenant.SingleOrDefaultAsync(x => x.Id == id);
      if (tenantToDelete != null)
      {
        _rentContext.Tenant.Remove(tenantToDelete);
        await _rentContext.SaveChangesAsync();
      }
    }

 
    public async Task Update(Tenant entity)
    {
      var tenantToUpdate = await _rentContext.Tenant
        .Include(x => x.Flat)
        .SingleOrDefaultAsync(x => x.Id == entity.Id);

      if (tenantToUpdate != null)
      {
        tenantToUpdate.Flat = entity.Flat;

        tenantToUpdate.DateOfUpdate = DateTime.Now;
        await _rentContext.SaveChangesAsync();
      }
    }
  }
}
