// <copyright file="ConfigurationMissingException.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Exceptions
{
  /// <summary>
  /// Represents an error when critical configuration is missing.
  /// </summary>
  public class ConfigurationMissingException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    public ConfigurationMissingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    /// <param name="message">The error message string.</param>
    public ConfigurationMissingException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    /// <param name="message">The error message string.</param>
    /// <param name="inner">The inner exception reference.</param>
    public ConfigurationMissingException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
