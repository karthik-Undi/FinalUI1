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
    
    public partial class FriendsAndFamily
    {
        public int FaFID { get; set; }
        public string FaFName { get; set; }
        public Nullable<int> ResidentID { get; set; }
        public string FaFRelation { get; set; }
    
        public virtual Resident Resident { get; set; }
    }
}
