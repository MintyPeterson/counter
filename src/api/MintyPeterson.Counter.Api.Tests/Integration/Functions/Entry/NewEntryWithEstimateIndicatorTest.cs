// <copyright file="NewEntryWithEstimateIndicatorTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration.Functions.Entry
{
  using System.Net;
  using System.Net.Http.Json;
  using FluentAssertions;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Models.Responses;
  using Xunit;

  /// <summary>
  /// Tests if the user can create a new entry while indicating that the entry is an estimate.
  /// </summary>
  public class NewEntryWithEstimateIndicatorTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private HttpResponseMessage? response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private EntryNewResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewEntryWithEstimateIndicatorTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public NewEntryWithEstimateIndicatorTest(CounterWebApplicationFactory fixture)
      : base(fixture)
    {
    }

    /// <summary>
    /// Tests if the status code is OK (200).
    /// </summary>
    [Fact]
    public void StatusCodeShouldBeOk() =>
      this.response!.StatusCode.Should().Be(HttpStatusCode.OK);

    /// <summary>
    /// Tests if the entry identifier is not empty.
    /// </summary>
    [Fact]
    public void EntryIdentiferShouldNotBeEmpty() =>
      this.responseContent!.EntryId.Should().NotBeEmpty();

    /// <inheritdoc/>
    public override Task InitializeAsync()
    {
      return this.SendRequestAsync();
    }

    /// <summary>
    /// Sends a request to the system.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    private async Task SendRequestAsync()
    {
      await this.ResetDatabaseAsync();

      var content = BuildRequestContent(
        new EntryNewRequest
        {
          EntryDate = DateTime.Today,
          Entry = 10,
          IsEstimate = true,
        });

      this.response = await this.Client.PostAsync("/Entry", content);

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          await this.response.Content.ReadFromJsonAsync<EntryNewResponse>();
      }
    }
  }
}
