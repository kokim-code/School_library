using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.Web.Models;

public enum CopyStatus
{
    Available = 1,
    Issued = 2,
    Lost = 3,
    Repair = 4
}

public sealed class Author
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string FullName { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = [];
}

public sealed class Genre
{
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = string.Empty;

    public List<Book> Books { get; set; } = [];
}

public sealed class Book
{
    public int Id { get; set; }

    [Required, MaxLength(220)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Isbn { get; set; }

    public int PublishYear { get; set; }

    public int AuthorId { get; set; }
    public Author? Author { get; set; }

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    public List<BookCopy> Copies { get; set; } = [];
}

public sealed class StorageLocation
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    public List<BookCopy> Copies { get; set; } = [];
}

public sealed class Reader
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string FullName { get; set; } = string.Empty;

    [Required, MaxLength(40)]
    public string ReaderType { get; set; } = "Ученик";

    [MaxLength(20)]
    public string? ClassName { get; set; }

    [MaxLength(40)]
    public string? Phone { get; set; }

    public List<Loan> Loans { get; set; } = [];
}

public sealed class BookCopy
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string InventoryNumber { get; set; } = string.Empty;

    public CopyStatus Status { get; set; } = CopyStatus.Available;

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int StorageLocationId { get; set; }
    public StorageLocation? StorageLocation { get; set; }

    public List<Loan> Loans { get; set; } = [];
    public List<Transfer> Transfers { get; set; } = [];
}

public sealed class Loan
{
    public int Id { get; set; }

    public int BookCopyId { get; set; }
    public BookCopy? BookCopy { get; set; }

    public int ReaderId { get; set; }
    public Reader? Reader { get; set; }

    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public bool IsClosed => ReturnDate.HasValue;
}

public sealed class Transfer
{
    public int Id { get; set; }

    public int BookCopyId { get; set; }
    public BookCopy? BookCopy { get; set; }

    public int FromLocationId { get; set; }
    public StorageLocation? FromLocation { get; set; }

    public int ToLocationId { get; set; }
    public StorageLocation? ToLocation { get; set; }

    public DateTime TransferDate { get; set; } = DateTime.UtcNow;

    [MaxLength(250)]
    public string? Comment { get; set; }
}
