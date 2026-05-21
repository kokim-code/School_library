using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;
using SchoolLibrary.Web.Services;

namespace SchoolLibrary.Tests;

public sealed class LoanServiceTests
{
    [Fact]
    public async Task DuplicateLoan_IsBlocked()
    {
        var db = CreateDb();
        db.Authors.Add(new Author { Id = 1, FullName = "Автор" });
        db.Genres.Add(new Genre { Id = 1, Name = "Жанр" });
        db.StorageLocations.Add(new StorageLocation { Id = 1, Name = "Фонд" });
        db.Books.Add(new Book { Id = 1, Title = "Книга", AuthorId = 1, GenreId = 1, PublishYear = 2024 });
        db.Readers.Add(new Reader { Id = 1, FullName = "Читатель", ReaderType = "Ученик" });
        db.BookCopies.Add(new BookCopy { Id = 1, BookId = 1, InventoryNumber = "LIB-T1", StorageLocationId = 1, Status = CopyStatus.Issued });
        await db.SaveChangesAsync();

        var service = new LoanService(db);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.IssueAsync(1, 1, DateTime.Today.AddDays(14)));
    }

    private static LibraryDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new LibraryDbContext(options);
    }
}
