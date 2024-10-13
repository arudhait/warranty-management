using System;
using System.Collections.Generic;

namespace Warranty.Repository.Models;

public partial class BreakdownStatusMast
{
    public short BreakdownStatusId { get; set; }

    public string? BreakdownStatusName { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string Ip { get; set; } = null!;

    public virtual ICollection<BreakdownDet> BreakdownDets { get; set; } = new List<BreakdownDet>();

    public virtual UserMast CreatedByNavigation { get; set; } = null!;

    public virtual UserMast? UpdatedByNavigation { get; set; }
}
