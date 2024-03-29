using Chinook.Domain.ApiModels;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public async Task<PagedList<AlbumApiModel>> GetAllAlbum(int pageNumber, int pageSize)
    {
        var albums = await albumRepository!.GetAll(pageNumber, pageSize);
        var albumApiModels = albums.ConvertAll<AlbumApiModel>().ToList();

        foreach (var album in albumApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Album-", album.Id), album, (TimeSpan)cacheEntryOptions);
        }

        var newPagedList =
            new PagedList<AlbumApiModel>(albumApiModels, albums.TotalCount, albums.CurrentPage, albums.PageSize);
        return newPagedList;
    }

    public async Task<AlbumApiModel?> GetAlbumById(int? id)
    {
        var albumApiModelCached = memoryCache!.Get<AlbumApiModel>(string.Concat("Album-", id));

        if (albumApiModelCached != null)
        {
            return albumApiModelCached;
        }
        else
        {
            var album = await albumRepository!.GetById(id);
            if (album == null) return null;
            var albumApiModel = album.Convert();
            var result = (artistRepository!.GetById(album.ArtistId)).Result;
            if (result != null)
                albumApiModel.ArtistName = result.Name;
            //albumApiModel.Tracks = (await GetTrackByAlbumId(id) ?? Array.Empty<TrackApiModel>()).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);

            memoryCache!.Set(string.Concat("Album-", albumApiModel.Id), albumApiModel, (TimeSpan)cacheEntryOptions);

            return albumApiModel;
        }
    }

    public async Task<PagedList<AlbumApiModel>> GetAlbumByArtistId(int id, int pageNumber, int pageSize)
    {
        var albums = await albumRepository!.GetByArtistId(id, pageNumber, pageSize);
        var albumApiModels = albums.ConvertAll<AlbumApiModel>();
        var newPagedList = new PagedList<AlbumApiModel>(albumApiModels.ToList(), albums.TotalCount, albums.CurrentPage,
            albums.PageSize);
        return newPagedList;
    }

    public async Task<AlbumApiModel> AddAlbum(AlbumApiModel newAlbumApiModel)
    {
        await albumValidator.ValidateAndThrowAsync(newAlbumApiModel);

        var album = newAlbumApiModel.Convert();

        album = await albumRepository!.Add(album);
        newAlbumApiModel.Id = album.Id;
        return newAlbumApiModel;
    }

    public async Task<bool> UpdateAlbum(AlbumApiModel albumApiModel)
    {
        await albumValidator.ValidateAndThrowAsync(albumApiModel);

        var album = await albumRepository!.GetById(albumApiModel.Id);

        if (album is null) return false;

        album.Title = albumApiModel.Title;
        album.ArtistId = albumApiModel.ArtistId;

        return await albumRepository.Update(album);
    }

    public Task<bool> DeleteAlbum(int id)
        => albumRepository!.Delete(id);
}