using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Services;

public sealed class ReaderService(LibraryDbContext db)
{
    public Task<List<Reader>> GetAllAsync() => db.Readers
        .OrderBy(x => x.FullName)
        .ToListAsync();

    public async Task AddAsync(Reader reader)
    {
        if (string.IsNullOrWhiteSpace(reader.FullName))
            throw new InvalidOperationException("ФИО читателя обязательно");

        db.Readers.Add(reader);
        await db.SaveChangesAsync();
    }
}
