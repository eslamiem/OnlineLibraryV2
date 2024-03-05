using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineLibrary.Models;
public class BorrowTransaction
{
    [Key]
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "Rental Date")]
    public DateTime RentalDate { get;  set; }

    [DataType(DataType.DateTime)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get;  set; }

    public BorrowingState State { get; private set; }

    [ForeignKey("CodeNumber")]
    public  Book Book { get; set; }
    public int CodeNumber { get; set; }
    
    [ForeignKey("UserId")]
    public  CustomUser Borrower { get; set; }
    public string? UserId { get; set; }



    public BorrowTransaction()
    {
        RentalDate = DateTime.Now;
        EndDate = DateTime.Now.AddMonths(1);
        State = BorrowingState.InProgrees;
    }


    public void SetBorrower(CustomUser user)
    {
        if (user == null)
        {
            throw new Exception("User is empty.");
        }
        if (Borrower == user) return;
        Borrower = user;
    }

    public void SetBook(Book book)
    {
        if (book == null)
        {
            throw new Exception("Book is empty.");
        }
        if (Book == book) return;
        Book = book;
    }

    public void EndBorrowing()
    {
        EndDate = DateTime.Now;
        State = BorrowingState.Closed;
    }

    public void CancelBorrowing()
    {
        EndDate = DateTime.Now;
        State = BorrowingState.Cancelled;
    }
    public enum BorrowingState
    {
        InProgrees,
        Closed,
        Cancelled
    }
}

