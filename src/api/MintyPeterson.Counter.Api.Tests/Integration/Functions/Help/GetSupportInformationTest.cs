// <copyright file="GetSupportInformationTest.cs" company="Tom Cook">
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
  /// Tests if the user can get supporting information from the system or not.
  /// </summary>
  public class GetSupportInformationTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private readonly HttpResponseMessage response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private readonly HelpAboutResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetSupportInformationTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public GetSupportInformationTest(CounterWebApplicationFactory fixture)
      : base(fixture)
    {
      this.response = this.Client.GetAsync("/").Result;

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          this.response.Content.ReadFromJsonAsync<HelpAboutResponse>().Result;
      }
    }

    /// <summary>
    /// Tests if the status code is OK (200).
    /// </summary>
    [Fact]
    public void StatusCodeShouldBeOk() =>
      this.response.StatusCode.Should().Be(HttpStatusCode.OK);

    /// <summary>
    /// Tests if the version is in the correct format.
    /// </summary>
    [Fact]
    public void VersionShouldBeCorrectFormat() =>
      this.responseContent!.Version.Should().MatchRegex(
          @"^(0|[1-9]+[0-9]*)\.(0|[1-9]+[0-9]*)\.(0|[1-9]+[0-9]*)(-(0|[1-9A-Za-z-][0-9A-Za-z-]*)"
            + @"(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$");

    /// <summary>
    /// Tests if the product name is correct.
    /// </summary>
    [Fact]
    public void NameShouldBeCorrect() =>
      this.responseContent!.Name.Should().Be(Strings.DefaultProductName);

    /// <summary>
    /// Tests if the support information is correct.
    /// </summary>
    [Fact]
    public void SupportInformationShouldBeCorrect() =>
      this.responseContent!.SupportInformation.Should().Be(Strings.SupportInformation);
  }
}
