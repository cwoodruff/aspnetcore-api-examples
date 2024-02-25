using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Chinook.EFCoreData.Data;

namespace Chinook.EFCoreData.Repositories;

public class ArtistRepository(ChinookContext context) : BaseRepository<Artist>(context), IArtistRepository;