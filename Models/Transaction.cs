using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrary.Models;
public class BookTransaction
{
    public int Id { get; set; }
    [Required]
    public int BookCodeNumber { get; set; }
    [Required]
    public string? UserId { get; set; }
    public DateTime DueDate { get; set; }
    public bool Returned { get; set; }

    [ForeignKey("BookCodeNumber")]
    public Book? Book { get; set; }
    public virtual required ApplicationUser User { get; set; }
}
