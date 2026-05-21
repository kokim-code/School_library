using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Data;

public static class DbSeeder
{
    public static void Seed(LibraryDbContext db)
    {
        if (db.Books.Any()) return;

        var authors = new[]
        {
            new Author { FullName = "А. С. Пушкин" },
            new Author { FullName = "Л. Н. Толстой" },
            new Author { FullName = "Н. В. Гоголь" }
        };

        var genres = new[]
        {
            new Genre { Name = "Художественная литература" },
            new Genre { Name = "Учебная литература" },
            new Genre { Name = "История" }
        };

        var locations = new[]
        {
            new StorageLocation { Name = "Основной фонд" },
            new StorageLocation { Name = "Кабинет литературы" },
            new StorageLocation { Name = "Читальный зал" }
        };

        db.Authors.AddRange(authors);
        db.Genres.AddRange(genres);
        db.StorageLocations.AddRange(locations);
        db.SaveChanges();

        var books = new[]
        {
            new Book { Title = "Капитанская дочка", AuthorId = authors[0].Id, GenreId = genres[0].Id, Isbn = "978517", PublishYear = 1836 },
            new Book { Title = "Война и мир", AuthorId = authors[1].Id, GenreId = genres[0].Id, Isbn = "978538", PublishYear = 1869 },
            new Book { Title = "Ревизор", AuthorId = authors[2].Id, GenreId = genres[0].Id, Isbn = "978500", PublishYear = 1836 }
        };
        db.Books.AddRange(books);
        db.SaveChanges();

        db.BookCopies.AddRange(
            new BookCopy { BookId = books[0].Id, InventoryNumber = "LIB-0001", StorageLocationId = locations[0].Id },
            new BookCopy { BookId = books[0].Id, InventoryNumber = "LIB-0002", StorageLocationId = locations[1].Id },
            new BookCopy { BookId = books[1].Id, InventoryNumber = "LIB-0003", StorageLocationId = locations[0].Id },
            new BookCopy { BookId = books[2].Id, InventoryNumber = "LIB-0004", StorageLocationId = locations[2].Id });

        db.Readers.AddRange(
            new Reader { FullName = "Иванова Мария", ReaderType = "Ученик", ClassName = "7А" },
            new Reader { FullName = "Петров Алексей", ReaderType = "Ученик", ClassName = "8Б" },
            new Reader { FullName = "Смирнова Ольга", ReaderType = "Учитель", ClassName = "" });

        db.SaveChanges();
    }
}
