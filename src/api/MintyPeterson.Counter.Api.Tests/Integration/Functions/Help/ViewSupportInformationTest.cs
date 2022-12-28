// <copyright file="ViewSupportInformationTest.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Tests.Integration.Functions.Help
{
  using System.Net;
  using System.Net.Http.Json;
  using FluentAssertions;
  using MintyPeterson.Counter.Api.Resources;
  using MintyPeterson.Counter.Api.Responses;
  using Xunit;

  /// <summary>
  /// Tests if the user can view supporting information.
  /// </summary>
  public class ViewSupportInformationTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private HttpResponseMessage? response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private HelpAboutResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewSupportInformationTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public ViewSupportInformationTest(CounterWebApplicationFactory fixture)
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
    /// Tests if the version is a valid semantic version format.
    /// </summary>
    [Fact]
    public void VersionShouldBeSemVerFormat() =>
      this.responseContent!.Version.Should().MatchRegex(
          @"^(0|[1-9]+[0-9]*)\.(0|[1-9]+[0-9]*)\.(0|[1-9]+[0-9]*)(-(0|[1-9A-Za-z-][0-9A-Za-z-]*)"
            + @"(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$");

    /// <summary>
    /// Tests if the name is the product name.
    /// </summary>
    [Fact]
    public void NameShouldBeProductName() =>
      this.responseContent!.Name.Should().Be(Strings.DefaultProductName);

    /// <summary>
    /// Tests if the support information contains contact details.
    /// </summary>
    [Fact]
    public void SupportInformationShouldBeContactDetails() =>
      this.responseContent!.SupportInformation.Should().Be(Strings.SupportInformation);

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
      this.response = await this.Client.GetAsync("/");

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          await this.response.Content.ReadFromJsonAsync<HelpAboutResponse>();
      }
    }
  }
}
