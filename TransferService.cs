using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Services;

public sealed class TransferService(LibraryDbContext db)
{
    public Task<List<Transfer>> GetAllAsync() => db.Transfers
        .Include(x => x.BookCopy).ThenInclude(x => x!.Book)
        .Include(x => x.FromLocation)
        .Include(x => x.ToLocation)
        .OrderByDescending(x => x.TransferDate)
        .ToListAsync();

    public async Task MoveAsync(int copyId, int toLocationId, string? comment)
    {
        var copy = await db.BookCopies.Include(x => x.StorageLocation).FirstOrDefaultAsync(x => x.Id == copyId)
            ?? throw new InvalidOperationException("Экземпляр не найден");

        if (copy.StorageLocationId == toLocationId)
            throw new InvalidOperationException("Новое место совпадает с текущим");

        var fromLocationId = copy.StorageLocationId;
        copy.StorageLocationId = toLocationId;

        db.Transfers.Add(new Transfer
        {
            BookCopyId = copyId,
            FromLocationId = fromLocationId,
            ToLocationId = toLocationId,
            Comment = comment,
            TransferDate = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}
