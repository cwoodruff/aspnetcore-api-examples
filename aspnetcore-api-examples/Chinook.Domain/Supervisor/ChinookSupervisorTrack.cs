using Chinook.Domain.Extensions;
using FluentValidation;
using Chinook.Domain.ApiModels;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<TrackApiModel>> GetAllTrack(int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetAll(pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll().ToList();

        var newPagedList =
            new PagedList<TrackApiModel>(trackApiModels, tracks.TotalCount, tracks.CurrentPage, tracks.PageSize);
        return newPagedList;
    }

    public async Task<TrackApiModel?> GetTrackById(int? id)
    {
        var track = await trackRepository!.GetById(id);
        if (track == null) return null;
        var trackApiModel = track.Convert();
        trackApiModel.Genre = await GetGenreById(trackApiModel.GenreId);
        trackApiModel.Album = await GetAlbumById(trackApiModel.AlbumId);
        trackApiModel.MediaType = await GetMediaTypeById(trackApiModel.MediaTypeId);
        if (trackApiModel.Album != null) trackApiModel.AlbumName = trackApiModel.Album.Title;

        if (trackApiModel.MediaType != null) trackApiModel.MediaTypeName = trackApiModel.MediaType.Name;
        if (trackApiModel.Genre != null) trackApiModel.GenreName = trackApiModel.Genre.Name;

        return trackApiModel;
    }

    public async Task<TrackApiModel> AddTrack(TrackApiModel newTrackApiModel)
    {
        await trackValidator.ValidateAndThrowAsync(newTrackApiModel);

        var track = newTrackApiModel.Convert();

        await trackRepository!.Add(track);
        newTrackApiModel.Id = track.Id;
        return newTrackApiModel;
    }

    public async Task<bool> UpdateTrack(TrackApiModel trackApiModel)
    {
        await trackValidator.ValidateAndThrowAsync(trackApiModel);

        var track = await trackRepository!.GetById(trackApiModel.Id);

        if (track == null) return false;

        track.Name = trackApiModel.Name;
        track.AlbumId = trackApiModel.AlbumId;
        track.MediaTypeId = trackApiModel.MediaTypeId;
        track.GenreId = trackApiModel.GenreId;
        track.Composer = trackApiModel.Composer ?? string.Empty;
        track.Milliseconds = trackApiModel.Milliseconds;
        track.Bytes = trackApiModel.Bytes;
        track.UnitPrice = trackApiModel.UnitPrice;

        return await trackRepository.Update(track);
    }

    public Task<bool> DeleteTrack(int id)
        => trackRepository!.Delete(id);

    public async Task<PagedList<TrackApiModel>?> GetTrackByAlbumId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByAlbumId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<TrackApiModel>> GetTrackByGenreId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByGenreId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<TrackApiModel>> GetTrackByMediaTypeId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByMediaTypeId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<TrackApiModel>> GetTrackByPlaylistId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByPlaylistId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<TrackApiModel>> GetTrackByArtistId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByArtistId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }

    public async Task<PagedList<TrackApiModel>> GetTrackByInvoiceId(int id, int pageNumber, int pageSize)
    {
        var tracks = await trackRepository!.GetByInvoiceId(id, pageNumber, pageSize);
        var trackApiModels = tracks.ConvertAll();
        var newPagedList = new PagedList<TrackApiModel>(trackApiModels.ToList(), tracks.TotalCount, tracks.CurrentPage,
            tracks.PageSize);
        return newPagedList;
    }
}