using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Services;

public sealed class BookService(LibraryDbContext db)
{
    public Task<List<Book>> SearchAsync(string? query) => db.Books
        .Include(x => x.Author)
        .Include(x => x.Genre)
        .Include(x => x.Copies)
        .Where(x => string.IsNullOrWhiteSpace(query)
            || x.Title.ToLower().Contains(query.ToLower())
            || x.Author!.FullName.ToLower().Contains(query.ToLower())
            || (x.Isbn != null && x.Isbn.Contains(query)))
        .OrderBy(x => x.Title)
        .ToListAsync();

    public Task<List<Author>> GetAuthorsAsync() => db.Authors.OrderBy(x => x.FullName).ToListAsync();
    public Task<List<Genre>> GetGenresAsync() => db.Genres.OrderBy(x => x.Name).ToListAsync();

    public async Task AddAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new InvalidOperationException("Название книги обязательно");

        db.Books.Add(book);
        await db.SaveChangesAsync();
    }
}
