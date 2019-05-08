using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RentApp.Infrastructure.Repository;
using RentApp.Infrastructure.Repository.OwnerRepository;

namespace RentApp.Infrastructure.Controllers
{
  
  [Route("api/[controller]")]
  [ApiController]
  public class OwnerController : ControllerBase
  {
    private readonly IOwnerRepository _ownerRepository;

    [HttpGet]
    public Task<IEnumerable<Owner>> getAll()
    {
      Task<IEnumerable<Owner>> owners = _ownerRepository.GetAll();
      
      return owners;
    }
    
    [HttpPost]
    public void  addOWner(Owner owner)
    {
      _ownerRepository.Add(owner);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<string>> Get()
    {
      return new string[] {"value1", "value2"};
    }
  }
}
