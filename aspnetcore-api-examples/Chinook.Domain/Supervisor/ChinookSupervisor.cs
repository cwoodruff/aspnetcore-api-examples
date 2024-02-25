using Chinook.Domain.ApiModels;
using Chinook.Domain.Repositories;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor(
    IAlbumRepository? albumRepository,
    IArtistRepository? artistRepository,
    ICustomerRepository? customerRepository,
    IEmployeeRepository? employeeRepository,
    IGenreRepository? genreRepository,
    IInvoiceLineRepository? invoiceLineRepository,
    IInvoiceRepository? invoiceRepository,
    IMediaTypeRepository? mediaTypeRepository,
    IPlaylistRepository? playlistRepository,
    ITrackRepository? trackRepository,
    IValidator<AlbumApiModel>? albumValidator,
    IValidator<ArtistApiModel>? artistValidator,
    IValidator<CustomerApiModel>? customerValidator,
    IValidator<EmployeeApiModel>? employeeValidator,
    IValidator<GenreApiModel>? genreValidator,
    IValidator<InvoiceApiModel>? invoiceValidator,
    IValidator<InvoiceLineApiModel>? invoiceLineValidator,
    IValidator<MediaTypeApiModel>? mediaTypeValidator,
    IValidator<PlaylistApiModel>? playlistValidator,
    IValidator<TrackApiModel>? trackValidator,
    IMemoryCache? memoryCache)
    : IChinookSupervisor;