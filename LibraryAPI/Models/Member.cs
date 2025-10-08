using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Models;

public partial class Member
{
    internal string Phone;

    [Key]
    public int MemberId { get; set; }

    [StringLength(225)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string? Email { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime JoinDate { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
}
