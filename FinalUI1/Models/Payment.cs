//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalUI1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Payment
    {
        public int PaymentID { get; set; }
        public string PaymentFor { get; set; }
        public string Amount { get; set; }
        public Nullable<int> ResidentID { get; set; }
        public Nullable<int> AmountPaidTo { get; set; }
        public string PaymentStatus { get; set; }
        public Nullable<int> ServiceID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Resident Resident { get; set; }
    }
}
