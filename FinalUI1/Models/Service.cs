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
    
    public partial class Service
    {
        public int ServiceID { get; set; }
        public int ResidentID { get; set; }
        public string ServiceType { get; set; }
        public Nullable<System.DateTime> AppointmentDateTime { get; set; }
        public string ServiceApproval { get; set; }
        public string ServiceMessage { get; set; }
        public Nullable<int> EmployeeID { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual Resident Resident { get; set; }
    }
}