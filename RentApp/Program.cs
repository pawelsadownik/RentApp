using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RentApp.Infrastructure;
using RentApp.Infrastructure.Context;

namespace RentApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
     
      using (var context = new RentContext()) {

        var std  = new Owner(){ FirstName = "aaa"};

        context.Owner.Add(std);
        context.SaveChanges();
      }
   Console.WriteLine("ssssssssssssssss");
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>();
  }
}
