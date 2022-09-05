// <copyright file="HelpAboutResponse.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Responses
{
  /// <summary>
  /// Provides a response model for information about the API.
  /// </summary>
  public class HelpAboutResponse
  {
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the support information.
    /// </summary>
    public string SupportInformation { get; set; } = string.Empty;
  }
}