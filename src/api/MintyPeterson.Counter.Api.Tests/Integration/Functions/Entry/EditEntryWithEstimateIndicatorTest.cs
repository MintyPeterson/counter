// <copyright file="EditEntryWithEstimateIndicatorTest.cs" company="Tom Cook">
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
  /// Tests if the user can edit an existing entry while indicating that the entry is an estimate.
  /// </summary>
  public class EditEntryWithEstimateIndicatorTest : IntegrationTest
  {
    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/>.
    /// </summary>
    private HttpResponseMessage? response;

    /// <summary>
    /// Stores the <see cref="HttpResponseMessage"/> content.
    /// </summary>
    private EntryEditResponse? responseContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditEntryWithEstimateIndicatorTest"/> class.
    /// </summary>
    /// <param name="fixture">A <see cref="CounterWebApplicationFactory"/>.</param>
    public EditEntryWithEstimateIndicatorTest(CounterWebApplicationFactory fixture)
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
      await this.ResetAndPopulateDatabaseAsync(
        @"
          DECLARE @CurrentDateTime datetimeoffset = SYSDATETIMEOFFSET()
          DECLARE @CurrentDate date = GETDATE()
          DECLARE @UserID nvarchar(450) = '00000000-0000-0000-0000-000000000001'
          DECLARE @EntryID nvarchar(450) = '00000000-0000-0000-0000-000000000001'

          INSERT INTO Users (
            UserID
           ,CreatedDateTime
           ,UpdatedDateTime
           ,Name
           ,Email
          )
          VALUES (
            @UserID
           ,@CurrentDateTime
           ,@CurrentDateTime
           ,'Integration Tester'
           ,'integration.tester@drtomcook.uk'
          )

          INSERT INTO Entries (
            EntryID
           ,CreatedDateTime
           ,CreatedByUserID
           ,UpdatedDateTime
           ,UpdatedByUserID
           ,EntryDate
           ,Entry
           ,IsEstimate
          )
          VALUES (
            @EntryID
           ,@CurrentDateTime
           ,@UserID
           ,@CurrentDateTime
           ,@UserID
           ,@CurrentDate
           ,'10'
           ,0
          )
        ");

      var content = BuildRequestContent(
        new EntryEditRequestBody
        {
          EntryDate = DateTime.Today.AddDays(1),
          Entry = 20,
          IsEstimate = true,
        });

      this.response =
        await this.Client.PutAsync("/Entry/00000000-0000-0000-0000-000000000001", content);

      if (this.response.IsSuccessStatusCode)
      {
        this.responseContent =
          await this.response.Content.ReadFromJsonAsync<EntryEditResponse>();
      }
    }
  }
}
