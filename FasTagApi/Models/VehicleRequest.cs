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
    
    public partial class VehicleRequest
    {
        public string InstitutionName { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public string WhatsappMobileNumber { get; set; }
        public string VehicleFullNumber { get; set; }
        public string VehicleType { get; set; }
        public string TravellingFromTO { get; set; }
        public string DepartmentInChargePermission { get; set; }
        public Nullable<decimal> RechargeAmount { get; set; }
        public Nullable<System.DateTime> DateOfRequest { get; set; }
        public string ReferenceNo { get; set; }
        public string Status { get; set; }
        public string Expiration { get; set; }
        public string FastagAccount { get; set; }
        public string Remarks { get; set; }
        public int Sno { get; set; }
    }
}
