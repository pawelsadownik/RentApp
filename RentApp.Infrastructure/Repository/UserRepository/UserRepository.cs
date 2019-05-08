using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository.UserRepository
{
  public class UserRepository : IUserRepository
  {
    private readonly RentContext _rentContext;

    public UserRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }
    
    public async Task<IEnumerable<User>> GetAll()  
    {
      var users = await _rentContext.User.ToListAsync();
      users.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Username).LoadAsync();});
      users.ForEach(x => { _rentContext.Entry(x).Reference(y => y.PasswordHash).LoadAsync();});
     
      return users;
    }

   
    public async Task<User> GetById(long id)
    {
      var user = await _rentContext.User
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();
      await _rentContext.Entry(user).Reference(x => x.Username).LoadAsync();
      await _rentContext.Entry(user).Reference(x => x.PasswordHash).LoadAsync();
        
      return user;
    }

    public async Task Add(User user)
    {
      user.DateOfCreation = DateTime.Now;
      await _rentContext.User
        .Include(x => x.Username)
        .Include(x => x.PasswordHash)
        .Include(x => x.IsActive)
        .FirstAsync();
      await _rentContext.User.AddAsync(user);
      await _rentContext.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
      var userToDelete = await _rentContext.User.SingleOrDefaultAsync(x => x.Id == id);
      if (userToDelete != null)
      {
        _rentContext.User.Remove(userToDelete);
        await _rentContext.SaveChangesAsync();
      }
    }

  

    public async Task Update(User entity)
    {
      var userToUpdate = await _rentContext.User
        .Include(x => x.Username)
        .Include(x => x.PasswordHash)
        .Include(x => x.IsActive)
        .SingleOrDefaultAsync(x => x.Id == entity.Id);

      if (userToUpdate != null)
      {
        userToUpdate.Username = entity.Username;
        userToUpdate.PasswordHash = entity.PasswordHash;
        userToUpdate.IsActive = entity.IsActive;
   
        userToUpdate.DateOfUpdate = DateTime.Now;
        await _rentContext.SaveChangesAsync();
      }
    }
  }
}
