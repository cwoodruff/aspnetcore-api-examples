using Chinook.Domain.Entities;
using Chinook.Domain.Repositories;
using Chinook.EFCoreData.Data;

namespace Chinook.EFCoreData.Repositories;

public class GenreRepository(ChinookContext context) : BaseRepository<Genre>(context), IGenreRepository
{
    public void Dispose() => _context.Dispose();
}