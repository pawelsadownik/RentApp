namespace RentApp.Infrastructure
{
  public class Image : Entity
  {
    public byte[] Data { get; set; }
    public Flat Flat { get; set; }
  }
}
