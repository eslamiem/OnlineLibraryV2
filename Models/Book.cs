using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrary.Models;
public class Book
{
    [Key]
    public int CodeNumber { get; set; }

    [Required]
    public string? Author { get; set; }

    [Required]
    public string? Title { get; set; }

    [DisplayName("Year Published")]
    [Range(1000, 9999, ErrorMessage = "Year Published must be between 1000 and 9999")]
    public int YearPublished { get; set; }
    public int Quantity { get; set; }
    public Availability Available { get; set; }

    public Book()
    {
        Available = Availability.Available;
    }

    public enum Availability
    {
        NotAvailable,
        Available
    }

    public void IsNotAvailable()
    {
        Available = Availability.NotAvailable;
    }

    public void IsAvailable()
    {
        Available = Availability.Available;
    }
    public static IQueryable<Book> GetBooks()
    {
        List<Book> books = new List<Book>() {
            new Book
            {
                CodeNumber = 1,
                Author = "Andrew Chevallier",
                Title = "Encyclopedia of Herbal Medicine: 550 Herbs and Remedies for Common Ailments",
                YearPublished = 2016,
                Quantity = 1,
                Available = Book.Availability.Available,
            },
            new Book
            {
                CodeNumber = 2,
                Author = "Michael T. Murray M.D. and Joseph Pizzorno",
                Title = "The Encyclopedia of Natural Medicine Third Edition",
                YearPublished = 2012,
                Quantity = 3,
                Available = Book.Availability.Available,
            },
            new Book
            {
                CodeNumber = 3,
                Author = "Thomas Easley and Steven Horne",
                Title = "The Modern Herbal Dispensatory: A Medicine-Making Guide",
                YearPublished = 2016,
                Quantity = 1,
                Available = Book.Availability.Available,
            },
            new Book
            {
                CodeNumber = 4,
                Author = "Cat Ellis",
                Title = "Prepper's Natural Medicine: Life-Saving Herbs, Essential Oils and Natural Remedies for When There is No Doctor",
                YearPublished = 2015,
                Quantity = 2,
                Available = Book.Availability.Available,
            },
            new Book
            {
                CodeNumber = 5,
                Author = "Rosemary Gladstar",
                Title = "Rosemary Gladstar's Medicinal Herbs: A Beginner's Guide: 33 Healing Herbs to Know, Grow, and Use",
                YearPublished = 2012,
                Quantity = 1,
                Available = Book.Availability.Available,
            },
        };

        return books.AsQueryable();

    }
}
