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
}
