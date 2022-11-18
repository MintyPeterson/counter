// <copyright file="EntryProfile.cs" company="Tom Cook">
// Copyright (c) Tom Cook. All rights reserved.
// </copyright>

namespace MintyPeterson.Counter.Api.Maps
{
  using AutoMapper;
  using MintyPeterson.Counter.Api.Models.Requests;
  using MintyPeterson.Counter.Api.Models.Responses;
  using MintyPeterson.Counter.Api.Services.Storage.Queries;
  using MintyPeterson.Counter.Api.Services.Storage.Results;

  /// <summary>
  /// Provides a mapping <see cref="Profile"/> for entry objects.
  /// </summary>
  public class EntryProfile : Profile
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EntryProfile"/> class.
    /// </summary>
    public EntryProfile()
    {
      this.MapNewAction();
      this.MapDeleteAction();
      this.MapListAction();
    }

    /// <summary>
    /// Maps <see cref="Controllers.EntryController.NewAsync"/>.
    /// </summary>
    private void MapNewAction()
    {
      this.CreateMap<EntryNewRequest, EntryNewQuery>()
        .ForMember(
          m => m.CreatedDateTime,
          m => m.Ignore())
        .ForMember(
          m => m.CreatedByUserId,
          m => m.Ignore());

      this.CreateMap<EntryNewResult, EntryNewResponse>();
    }

    /// <summary>
    /// Maps <see cref="Controllers.EntryController.DeleteAsync"/>.
    /// </summary>
    private void MapDeleteAction()
    {
      this.CreateMap<EntryDeleteRequest, EntryGetQuery>();
      this.CreateMap<EntryDeleteRequest, EntryDeleteQuery>();

      this.CreateMap<EntryDeleteRequest, EntryDeleteQuery>()
        .ForMember(
          m => m.DeletedDateTime,
          m => m.Ignore())
        .ForMember(
          m => m.DeletedByUserId,
          m => m.Ignore());

      this.CreateMap<EntryDeleteResult, EntryDeleteResponse>();
    }

    /// <summary>
    /// Maps <see cref="Controllers.EntryController.ListAsync"/>.
    /// </summary>
    private void MapListAction()
    {
      this.CreateMap<EntryListRequest, EntryListQuery>();
      this.CreateMap<EntryListEntryResult, EntryListEntryResponse>();
      this.CreateMap<EntryListResult, EntryListResponse>();
    }
  }
}
