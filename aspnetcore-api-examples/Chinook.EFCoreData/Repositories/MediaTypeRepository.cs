using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Chinook.EFCoreData.Data;

namespace Chinook.EFCoreData.Repositories;

public class MediaTypeRepository(ChinookContext context) : BaseRepository<MediaType>(context), IMediaTypeRepository;