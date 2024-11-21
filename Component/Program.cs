using System;
using System.Collections.Generic;
using System.Linq;

public interface IAccountingSystem
{
    void IssueBook(Book book, Reader reader);
    void ReturnBook(Book book, Reader reader);
}

public interface ICatalog
{
    Book SearchBook(string searchCriteria);
    List<Book> FilterBooksByGenre(string genre);
}

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string ISBN { get; set; }

    public Book(string title, string author, string genre, string isbn)
    {
        Title = title;
        Author = author;
        Genre = genre;
        ISBN = isbn;
    }
}

public class Reader
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string TicketNumber { get; set; }

    public Reader(string firstName, string lastName, string ticketNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        TicketNumber = ticketNumber;
    }
}

public class Librarian
{
    private IAccountingSystem _accountingSystem;

    public Librarian(IAccountingSystem accountingSystem)
    {
        _accountingSystem = accountingSystem;
    }

    public void IssueBook(Book book, Reader reader)
    {
        _accountingSystem.IssueBook(book, reader);
        Console.WriteLine($"Книга '{book.Title}' выдана читателю {reader.FirstName} {reader.LastName}");
    }

    public void ReturnBook(Book book, Reader reader)
    {
        _accountingSystem.ReturnBook(book, reader);
        Console.WriteLine($"Книга '{book.Title}' возвращена читателем {reader.FirstName} {reader.LastName}");
    }
}

public class Catalog : ICatalog
{
    private List<Book> _books;

    public Catalog(List<Book> books)
    {
        _books = books;
    }

    public Book SearchBook(string searchCriteria)
    {
        return _books.FirstOrDefault(b => b.Title.Contains(searchCriteria) || b.Author.Contains(searchCriteria));
    }

    public List<Book> FilterBooksByGenre(string genre)
    {
        return _books.Where(b => b.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}

public class AccountingSystem : IAccountingSystem
{
    private Dictionary<Book, Reader> _issuedBooks;

    public AccountingSystem()
    {
        _issuedBooks = new Dictionary<Book, Reader>();
    }

    public void IssueBook(Book book, Reader reader)
    {
        if (!_issuedBooks.ContainsKey(book))
        {
            _issuedBooks.Add(book, reader);
        }
        else
        {
            Console.WriteLine($"Книга '{book.Title}' уже выдана.");
        }
    }

    public void ReturnBook(Book book, Reader reader)
    {
        if (_issuedBooks.ContainsKey(book) && _issuedBooks[book] == reader)
        {
            _issuedBooks.Remove(book);
        }
        else
        {
            Console.WriteLine($"Ошибка при возврате книги '{book.Title}'.");
        }
    }
}
class LibrarySystem
{
    static void Main(string[] args)
    {
        var book1 = new Book("Война и мир", "Лев Толстой", "Роман", "978-5-00-011019-9");
        var book2 = new Book("Преступление и наказание", "Фёдор Достоевский", "Роман", "978-5-00-011020-5");

        var reader = new Reader("Алинур", "Шаяхметов", "12345");

        var catalog = new Catalog(new List<Book> { book1, book2 });
        var accountingSystem = new AccountingSystem();
        var librarian = new Librarian(accountingSystem);
        librarian.IssueBook(book1, reader);
        librarian.ReturnBook(book1, reader);
    }
}
