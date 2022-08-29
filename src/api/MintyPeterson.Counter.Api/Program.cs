// <copyright file="Program.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api
{
  using Microsoft.AspNetCore;

  /// <summary>
  /// Defines the application.
  /// </summary>
  public class Program
  {
    /// <summary>
    /// Provides an entry point for the application.
    /// </summary>
    /// <param name="arguments">The command line arguments.</param>
    public static void Main(string[] arguments)
    {
      Program.CreateHostBuilder(arguments).Build().Run();
    }

    /// <summary>
    /// Creates the <see cref="IWebHostBuilder"/> for the application.
    /// </summary>
    /// <param name="arguments">The command line arguments.</param>
    /// <returns>A <see cref="IWebHostBuilder"/>.</returns>
    /// <remarks>This method is called by Entity Framework migrations.</remarks>
    public static IWebHostBuilder CreateHostBuilder(string[] arguments)
    {
      return WebHost.CreateDefaultBuilder(arguments).UseStartup<Startup>();
    }
  }
}
