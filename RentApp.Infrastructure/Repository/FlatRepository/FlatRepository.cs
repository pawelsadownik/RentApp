using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository
{
  public class FlatRepository : IFlatRepository
 
  {
    
    private readonly RentContext _rentContext;

    public FlatRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }
    
    public async Task<IEnumerable<Flat>> GetAll()  
    {
      var flats = await _rentContext.Flat.ToListAsync();
      flats.ForEach(x => { _rentContext.Entry(x).Reference(y => y.District).LoadAsync();});
      flats.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Address).LoadAsync();});
      flats.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Owner).LoadAsync();});
      flats.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Tenant).LoadAsync();});
      
      // dociaganie kolekcji poprzez Collection czy Reference?????????
      flats.ForEach(x => { _rentContext.Entry(x).Collection(y => y.Images).LoadAsync();});
    
      return flats;
    }

   
    public async Task<Flat> GetById(long id)
    {
      var flat = await _rentContext.Flat
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();
      await _rentContext.Entry(flat).Reference(x => x.Address).LoadAsync();
      await _rentContext.Entry(flat).Reference(x => x.District).LoadAsync();
      await _rentContext.Entry(flat).Reference(x => x.Owner).LoadAsync();
      await _rentContext.Entry(flat).Reference(x => x.Tenant).LoadAsync();

      //collection
      await _rentContext.Entry(flat).Collection(x => x.Images).LoadAsync();
     
      return flat;
    }

    public async Task Add(Flat flat)
    {
      flat.DateOfCreation = DateTime.Now;
      await _rentContext.Flat
        .Include(x => x.Address)
        .Include(x => x.Owner)
        .Include(x => x.Tenant)
        .Include(x => x.Images)
        .FirstAsync();
      await _rentContext.Flat.AddAsync(flat);
      await _rentContext.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
      var flatToDelete = await _rentContext.Flat.SingleOrDefaultAsync(flat => flat.Id == id);
      if (flatToDelete != null)
      {
        _rentContext.Flat.Remove(flatToDelete);
        await _rentContext.SaveChangesAsync();
      }
    }

  

    public async Task Update(Flat entity)
    {
      var flatToUpdate = await _rentContext.Flat
        .Include(x => x.Address)
        .Include(x => x.Owner)
        .Include(x => x.Tenant)
        .Include(x => x.Images)
        .SingleOrDefaultAsync(x => x.Id == entity.Id);

      if (flatToUpdate != null)
      {
        flatToUpdate.Owner = entity.Owner;
        flatToUpdate.Images = entity.Images;
        flatToUpdate.Tenant = entity.Tenant;
        flatToUpdate.Address = entity.Address;
        flatToUpdate.Floor = entity.Floor;
        flatToUpdate.Price = entity.Price;

        flatToUpdate.District = flatToUpdate.District;
        flatToUpdate.IsElevator = flatToUpdate.IsElevator;
        flatToUpdate.SquareMeters = flatToUpdate.SquareMeters;
        flatToUpdate.NumberOfRooms = flatToUpdate.NumberOfRooms;
        flatToUpdate.DateOfUpdate = DateTime.Now;
        await _rentContext.SaveChangesAsync();
      }
    }
    
    
  }
}
