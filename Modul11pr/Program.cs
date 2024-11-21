using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Library library = new Library();
        Author author = new Author("Лев Толстой");
        Book book = new Book("Война и мир", "123-456", author, 1869);
        library.AddBook(book);
        Reader reader = new Reader("Nurik", "nurik@mail.com", "001");
        library.RegisterUser(reader);
        Librarian librarian = new Librarian("Alinur", "alinur@mail.com");
        try
        {
            Loan loan = librarian.IssueLoan(book, reader);
            Console.WriteLine(loan.GetLoanInfo());

            librarian.ReturnBook(loan);
            Console.WriteLine($"Книга '{book.Title}' возвращена в библиотеку.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
class Author
{
    public string Name { get; set; }

    public Author(string name)
    {
        Name = name;
    }
}
class Book
{
    public string Title { get; set; }
    public string ISBN { get; set; }
    public Author Author { get; set; }
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; }

    public Book(string title, string isbn, Author author, int year, bool isAvailable = true)
    {
        Title = title;
        ISBN = isbn;
        Author = author;
        PublicationYear = year;
        IsAvailable = isAvailable;
    }

    public void ChangeAvailability(bool availability)
    {
        IsAvailable = availability;
    }
    public string GetBookInfo()
    {
        return $"{Title} by {Author.Name} (ISBN: {ISBN}, Year: {PublicationYear})";
    }
}
class User
{
    public string Name { get; set; }
    public string Email { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
class Reader : User
{
    public string TicketNumber { get; set; }

    public Reader(string name, string email, string ticketNumber)
        : base(name, email)
    {
        TicketNumber = ticketNumber;
    }
}
class Librarian : User
{
    public Librarian(string name, string email)
        : base(name, email) { }

    public Loan IssueLoan(Book book, Reader reader)
    {
        if (!book.IsAvailable)
            throw new Exception("Книга недоступна для выдачи.");

        book.ChangeAvailability(false);
        return new Loan(book, reader, DateTime.Now, null);
    }
    public void ReturnBook(Loan loan)
    {
        loan.Book.ChangeAvailability(true);
        loan.ReturnDate = DateTime.Now;
    }
}
class Loan
{
    public Book Book { get; set; }
    public Reader Reader { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }

    public Loan(Book book, Reader reader, DateTime loanDate, DateTime? returnDate)
    {
        Book = book;
        Reader = reader;
        LoanDate = loanDate;
        ReturnDate = returnDate;
    }
    public string GetLoanInfo()
    {
        return $"Книга '{Book.Title}' была выдана {Reader.Name} {LoanDate.ToShortDateString()}";
    }
}
class Library
{
    private List<Book> books = new List<Book>();
    private List<User> users = new List<User>();

    public void AddBook(Book book)
    {
        books.Add(book);
    }
    public void RegisterUser(User user)
    {
        users.Add(user);
    }
    public Book FindBook(string title)
    {
        return books.Find(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
    public List<Book> GetAvailableBooks()
    {
        return books.FindAll(b => b.IsAvailable);
    }
}
