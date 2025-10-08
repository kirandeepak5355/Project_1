using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models;

[Index("Isbn", Name = "UQ__Books__447D36EAD526E724", IsUnique = true)]
public partial class Book
{
    internal string ISBN;

    [Key]
    public int BookId { get; set; }

    [StringLength(225)]
    public string Title { get; set; } = null!;

    [StringLength(225)]
    public string Author { get; set; } = null!;

    [Column("ISBN")]
    [StringLength(13)]
    [Unicode(false)]
    public string Isbn { get; set; } = null!;
    //public string ISBN { get; internal set; }
    public int? PublishedYear { get; set; }

    public int AvailableCopies { get; set; }

    [InverseProperty("Book")]
    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
