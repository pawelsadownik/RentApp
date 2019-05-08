using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository.OwnerRepository
{
  public class OwnerRepository : IOwnerRepository
  {
    private readonly RentContext _rentContext;

    public OwnerRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }

    public async Task<IEnumerable<Owner>> GetAll()
    {
      var owners = await _rentContext.Owner.ToListAsync();
      owners.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Flats).LoadAsync();});

      return owners;
    }

    public async Task<Owner> GetById(long id)
    {
      var owner = await _rentContext.Owner
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();

      await _rentContext.Entry(owner).Collection(x => x.Flats).LoadAsync();

      return owner;
    }

    public async Task Add(Owner owner)
    {
      owner.DateOfCreation = DateTime.Now;

      await _rentContext.Owner
        .Include(x => x.Flats)
        .FirstAsync();
      await _rentContext.Owner.AddAsync(owner);
      await _rentContext.SaveChangesAsync();
    }

    public async Task Update(Owner entity)
    {
      var ownerToUpdate = await _rentContext.Owner
        .Include(x => x.Flats)
        .SingleOrDefaultAsync(x => x.Id == entity.Id);

      if (ownerToUpdate != null)
      {
        ownerToUpdate.Flats = entity.Flats;
        ownerToUpdate.DateOfUpdate = DateTime.Now;

    
        await _rentContext.SaveChangesAsync();
      }
      
    }

    public async Task Delete(long id)
    {
      var ownerToDelete = await _rentContext.Owner.SingleOrDefaultAsync(x => x.Id == id);
      if (ownerToDelete != null)
      {
        _rentContext.Owner.Remove(ownerToDelete);
        await _rentContext.SaveChangesAsync();
      }    }
  }
}
