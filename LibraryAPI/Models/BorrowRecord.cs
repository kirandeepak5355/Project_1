using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models;

public partial class BorrowRecord
{
    [Key]
    public int BorrowId { get; set; }

    public int MemberId { get; set; }

    public int BookId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime BorrowDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReturnDate { get; set; }

    public bool IsReturned { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("BorrowRecords")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("MemberId")]
    [InverseProperty("BorrowRecords")]
    public virtual Member Member { get; set; } = null!;
}
