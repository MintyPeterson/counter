// <copyright file="StorageServiceResult.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Services.Storage.Results
{
  /// <summary>
  /// Represents a <see cref="StorageService"/> result that supports returning an exception.
  /// </summary>
  /// <typeparam name="T">The result type.</typeparam>
  public class StorageServiceResult<T>
  {
    /// <summary>
    /// Gets a value indicating whether the query that provides the result has succeeded or not.
    /// </summary>
    public bool HasSucceeded => this.Result != null;

    /// <summary>
    /// Gets or sets the <see cref="Exception"/>.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets or sets the result.
    /// </summary>
    public T? Result { get; set; }
  }
}
