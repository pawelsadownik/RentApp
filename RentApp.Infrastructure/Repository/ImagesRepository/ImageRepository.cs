using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Infrastructure.Context;

namespace RentApp.Infrastructure.Repository.ImagesRepository
{
  public class ImageRepository : IImageRepository
  {
    private readonly RentContext _rentContext;

    
    public ImageRepository(RentContext rentContext)
    {
      _rentContext = rentContext;
    }
    
    public async Task<IEnumerable<Image>> GetAll()
    {
      var images = await _rentContext.Image.ToListAsync();
      images.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Data).LoadAsync();});
      images.ForEach(x => { _rentContext.Entry(x).Reference(y => y.Flat).LoadAsync();});

      return images;
    }

    public async Task<Image> GetById(long id)
    {
      var image = await _rentContext.Image
        .Where(x => x.Id == id)
        .SingleOrDefaultAsync();

      await _rentContext.Entry(image).Reference(x => x.Data).LoadAsync();
      await _rentContext.Entry(image).Reference(x => x.Data).LoadAsync();
      
      return image;
    }

    public async Task Add(Image image)
    {
     image.DateOfCreation = DateTime.Now;

     await _rentContext.Image
       .Include(x => x.Data)
       .Include(x => x.Flat)
       .FirstAsync();
    }

    public async Task Update(Image entity)
    {
      var imageToUpdate = await _rentContext.Image
        .Include(x => x.Data)
        .Include(x => x.Flat)
        .SingleOrDefaultAsync();

      if (imageToUpdate != null)
      {
        imageToUpdate.Data = entity.Data;
        imageToUpdate.Flat = entity.Flat;
        imageToUpdate.DateOfUpdate = DateTime.Now;

        await _rentContext.SaveChangesAsync();
      }
    }

    public async Task Delete(long id)
    {
      var imageToDelete = await _rentContext.Image.SingleOrDefaultAsync(x => x.Id == id);

      if (imageToDelete != null)
      {
        _rentContext.Image.Remove(imageToDelete);
        await _rentContext.SaveChangesAsync();
      }
    }
  }
}
