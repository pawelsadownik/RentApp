using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository.AddressesRepository
{
  public class AddressRepository : IAddressRepository
  {
    private readonly RentContext _rentContext;

    public AddressRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }
    
    
    public async Task<IEnumerable<Address>> GetAll()
    {
      var addresses = await _rentContext.Address.ToListAsync();
      addresses.ForEach(x => { _rentContext.Entry(x).Reference(y => y.City).LoadAsync();});
      addresses.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Street).LoadAsync();});
      addresses.ForEach(x => { _rentContext.Entry(x).Reference(y => y.ZipCode).LoadAsync();});
      addresses.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Country).LoadAsync();});

      return addresses;
    }

    public async Task<Address> GetById(long id)
    {
      var address = await _rentContext.Address
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();

      await _rentContext.Entry(address).Reference(x => x.City).LoadAsync();
      await _rentContext.Entry(address).Reference(x => x.Street).LoadAsync();
      await _rentContext.Entry(address).Reference(x => x.ZipCode).LoadAsync();
      await _rentContext.Entry(address).Reference(x => x.Country).LoadAsync();

      return address;
    }

    public async Task Add(Address address)
    {
      address.DateOfCreation = DateTime.Now;

      await _rentContext.Address
        .Include(x => x.City)
        .Include(x => x.Street)
        .Include(x => x.ZipCode)
        .Include(x => x.Country)
        .FirstAsync();
      
      await _rentContext.Address.AddAsync(address);
      await _rentContext.SaveChangesAsync();
    }
    
    public async Task Delete(long id)
    {
      var addressToDelete = await _rentContext.Address.SingleOrDefaultAsync(x => x.Id == id);
      if (addressToDelete != null)
      {
        _rentContext.Address.Remove(addressToDelete);
        await _rentContext.SaveChangesAsync();
      }
      
    }
    
    public async Task Update(Address entity)
    {
      var addressToUpdate = await _rentContext.Address
        .Include(x => x.Street)
        .Include(x => x.City)
        .Include(x => x.ZipCode)
        .Include(x => x.Country)
        .SingleOrDefaultAsync(x => x.Id == entity.Id);

      if (addressToUpdate != null)
      {
        addressToUpdate.City = entity.City;
        addressToUpdate.Street = entity.Street;
        addressToUpdate.ZipCode = entity.ZipCode;
        addressToUpdate.Country = entity.Country;
        addressToUpdate.DateOfUpdate = DateTime.Now;

        await _rentContext.SaveChangesAsync();
      }
    }

    
  }
}
