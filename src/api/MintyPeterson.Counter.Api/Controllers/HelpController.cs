// <copyright file="HelpController.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Controllers
{
  using System.Diagnostics;
  using System.Reflection;
  using Microsoft.AspNetCore.Mvc;
  using MintyPeterson.Counter.Api.Resources;
  using MintyPeterson.Counter.Api.Responses;

  /// <summary>
  /// Provides actions for help routes.
  /// </summary>
  [ApiController]
  public class HelpController : ControllerBase
  {
    /// <summary>
    /// Returns information about the application.
    /// </summary>
    /// <returns>An <see cref="ActionResult{HelpAboutResponse}"/>.</returns>
    [HttpGet("/")]
    public ActionResult<HelpAboutResponse> About()
    {
      var assembly = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

      return
        new HelpAboutResponse
        {
          Name = assembly.ProductName ?? Strings.DefaultProductName,
          Version = assembly.ProductVersion ?? Strings.DefaultProductVersion,
          SupportInformation = Strings.SupportInformation,
        };
    }
  }
}