using System;
using System.Collections.Generic;
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
    public int YearPublished { get; set; }
    public int Quantity { get; set; }
}
