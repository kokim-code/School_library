using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Services;

public sealed class BookCopyService(LibraryDbContext db)
{
    public Task<List<BookCopy>> GetAllAsync() => db.BookCopies
        .Include(x => x.Book).ThenInclude(x => x!.Author)
        .Include(x => x.StorageLocation)
        .OrderBy(x => x.InventoryNumber)
        .ToListAsync();

    public Task<List<BookCopy>> GetAvailableAsync() => db.BookCopies
        .Include(x => x.Book)
        .Where(x => x.Status == CopyStatus.Available)
        .OrderBy(x => x.InventoryNumber)
        .ToListAsync();

    public Task<List<Book>> GetBooksAsync() => db.Books.OrderBy(x => x.Title).ToListAsync();
    public Task<List<StorageLocation>> GetLocationsAsync() => db.StorageLocations.OrderBy(x => x.Name).ToListAsync();

    public async Task AddAsync(BookCopy copy)
    {
        if (await db.BookCopies.AnyAsync(x => x.InventoryNumber == copy.InventoryNumber))
            throw new InvalidOperationException("Инвентарный номер уже используется");

        db.BookCopies.Add(copy);
        await db.SaveChangesAsync();
    }
}
