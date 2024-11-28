using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace Library_1
{
    // Book class
    public class Book
    {
        
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Book(string title, string author, DateTime releaseDate)
        {
            Title = title;
            Author = author;
            ReleaseDate = releaseDate;
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Release Date: {ReleaseDate.ToShortDateString()}";
        }
    }

    // BookManager class
    public class BookManager
    {
        private List<Book> books = new List<Book>();
        private string filePath;

        public BookManager(string filePath)
        {
            this.filePath = filePath;
            LoadBooks();
        }

        public void AddBook(Book book)
        {
            books.Add(book);
            Console.WriteLine("Book added successfully.");
        }

        public void ShowAllBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("No books available.");
                return;
            }

            Console.WriteLine("List of Books:");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        public void SearchBookByTitle(string title)
        {
            var foundBooks = books.FindAll(b => b.Title.IndexOf(title, StringComparison.OrdinalIgnoreCase) >= 0);
            if (foundBooks.Count == 0)
            {
                Console.WriteLine("No books found with that title.");
                return;
            }

            Console.WriteLine("Search Results:");
            foreach (var book in foundBooks)
            {
                Console.WriteLine(book);
            }
        }

        public void SaveBooks()
        {
            var json = JsonSerializer.Serialize(books);
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Books saved to {filePath}.");
        }

        private void LoadBooks()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
                Console.WriteLine("Books loaded from file.");
            }
        }
    }

    // Main program
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the file path to save/load books: ");
            string filePath = Console.ReadLine();

            BookManager bookManager = new BookManager(filePath);
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nBook Management System");
                Console.WriteLine("1. Add New Book");
                Console.WriteLine("2. Show All Books");
                Console.WriteLine("3. Search Book by Title");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewBook(bookManager);
                        break;
                    case "2":
                        bookManager.ShowAllBooks();
                        break;
                    case "3":
                        SearchBook(bookManager);
                        break;
                    case "4":
                        bookManager.SaveBooks(); // Save books before exiting
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddNewBook(BookManager bookManager)
        {
            Console.Write("Enter book title: ");
            string title = Console.ReadLine();

            Console.Write("Enter author name: ");
            string author = Console.ReadLine();

            Console.Write("Enter release date (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime releaseDate))
            {
                Book newBook = new Book(title, author, releaseDate);
                bookManager.AddBook(newBook);
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again.");
            }
        }

        static void SearchBook(BookManager bookManager)
        {
            Console.Write("Enter the title to search: ");
            string title = Console.ReadLine();
            bookManager.SearchBookByTitle(title);
        }
    }
}