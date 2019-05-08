using System.Collections.Generic;
using System.Threading.Tasks;
using RentApp.Contract.DTO;
using RentApp.Infrastructure.Repository;

namespace RentApp.Core.Services
{

  public interface IFlatService : IService<FlatDto>
  {
  }

  public class FlatService : IFlatService
  {

    private readonly IFlatRepository _iflatRepository;

    public FlatService(IFlatRepository flatRepository)
    {
      _iflatRepository = flatRepository;
    }
    

  }
}
