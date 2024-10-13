using System;
using System.Collections.Generic;

namespace Warranty.Repository.Models;

public partial class BreakdownDet
{
    public long BreakdownId { get; set; }

    public long CustId { get; set; }

    public DateTime CallRegDate { get; set; }

    public short TypeId { get; set; }

    public int EnggId { get; set; }

    public DateTime EnggFirstVisitDate { get; set; }

    public string CrmNo { get; set; } = null!;

    public string Problems { get; set; } = null!;

    public short ReqAction { get; set; }

    public short ActionTaken { get; set; }

    public short Conclusion { get; set; }

    public bool? IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ActionMast ActionTakenNavigation { get; set; } = null!;

    public virtual UserMast CreatedByNavigation { get; set; } = null!;

    public virtual CustMast Cust { get; set; } = null!;

    public virtual EnggMast Engg { get; set; } = null!;

    public virtual ActionMast ReqActionNavigation { get; set; } = null!;

    public virtual BreakdownStatusMast Type { get; set; } = null!;

    public virtual UserMast? UpdatedByNavigation { get; set; }
}
