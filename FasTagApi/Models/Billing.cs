//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FasTagApi.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Billing
    {
        public int SNO { get; set; }
        public string BillNo { get; set; }
        public System.DateTime BillDate { get; set; }
        public decimal Amount { get; set; }
        public string DeptCode { get; set; }
        public string Department { get; set; }
        public string PartyCode { get; set; }
        public string PartyName { get; set; }
        public string Mode { get; set; }
        public string RefNo { get; set; }
        public Nullable<System.DateTime> DateOfPayment { get; set; }
        public string ViewStatus { get; set; }

    }
}
