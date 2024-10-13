using System;
using System.Collections.Generic;

namespace Warranty.Repository.Models;

public partial class ContractDet
{
    public long ContractId { get; set; }

    public short ContractTypeId { get; set; }

    public long WarrantyId { get; set; }

    public decimal? Amount { get; set; }

    public string? ChequeDet { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal? AmtExcludTax { get; set; }

    public short? NoOfService { get; set; }

    public string? Interval { get; set; }

    public int? TakenBy { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ContractTypeMast ContractType { get; set; } = null!;

    public virtual UserMast? CreatedByNavigation { get; set; }

    public virtual EnggMast? TakenByNavigation { get; set; }

    public virtual WarrantyDet Warranty { get; set; } = null!;
}
