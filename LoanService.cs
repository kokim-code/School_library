using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Web.Data;
using SchoolLibrary.Web.Models;

namespace SchoolLibrary.Web.Services;

public sealed class LoanService(LibraryDbContext db)
{
    public Task<List<Loan>> GetAllAsync() => db.Loans
        .Include(x => x.Reader)
        .Include(x => x.BookCopy).ThenInclude(x => x!.Book)
        .OrderByDescending(x => x.IssueDate)
        .ToListAsync();

    public async Task IssueAsync(int copyId, int readerId, DateTime dueDate)
    {
        var copy = await db.BookCopies.FindAsync(copyId)
            ?? throw new InvalidOperationException("Экземпляр не найден");

        if (copy.Status == CopyStatus.Issued)
            throw new InvalidOperationException("Экземпляр уже выдан");

        copy.Status = CopyStatus.Issued;
        db.Loans.Add(new Loan
        {
            BookCopyId = copyId,
            ReaderId = readerId,
            DueDate = DateTime.SpecifyKind(dueDate, DateTimeKind.Utc),
            IssueDate = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }

    public async Task ReturnAsync(int loanId)
    {
        var loan = await db.Loans.Include(x => x.BookCopy).FirstOrDefaultAsync(x => x.Id == loanId)
            ?? throw new InvalidOperationException("Выдача не найдена");

        if (loan.ReturnDate.HasValue)
            throw new InvalidOperationException("Книга уже возвращена");

        loan.ReturnDate = DateTime.UtcNow;
        loan.BookCopy!.Status = CopyStatus.Available;
        await db.SaveChangesAsync();
    }
}
