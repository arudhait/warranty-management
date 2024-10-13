using System;
using System.Collections.Generic;

namespace Warranty.Repository.Models;

public partial class ActionMast
{
    public short ActionId { get; set; }

    public string ActionName { get; set; } = null!;

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string Ip { get; set; } = null!;

    public virtual ICollection<BreakdownDet> BreakdownDetActionTakenNavigations { get; set; } = new List<BreakdownDet>();

    public virtual ICollection<BreakdownDet> BreakdownDetReqActionNavigations { get; set; } = new List<BreakdownDet>();

    public virtual UserMast CreatedByNavigation { get; set; } = null!;

    public virtual UserMast? UpdatedByNavigation { get; set; }
}
