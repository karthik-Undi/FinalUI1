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
    
    public partial class Resident
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resident()
        {
            this.Complaints = new HashSet<Complaint>();
            this.DashboardPosts = new HashSet<DashboardPost>();
            this.FriendsAndFamilies = new HashSet<FriendsAndFamily>();
            this.Payments = new HashSet<Payment>();
            this.Services = new HashSet<Service>();
            this.Visitors = new HashSet<Visitor>();
            this.HouseLists = new HashSet<HouseList>();
        }

        public Resident(string name, string password, string email_Emp, string type, string mobileNo, int houseno)
        {
            ResidentEmail = email_Emp;
            ResidentMobileNo = mobileNo;
            ResidentName = name;
            ResidentPassword = password;
            ResidentType = type;
            ResidentHouseNo = houseno;
        }

        public int ResidentID { get; set; }
        public string ResidentName { get; set; }
        public Nullable<int> ResidentHouseNo { get; set; }
        public string ResidentType { get; set; }
        public string ResidentMobileNo { get; set; }
        public string ResidentEmail { get; set; }
        public string ResidentPassword { get; set; }
        public string isApproved { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DashboardPost> DashboardPosts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FriendsAndFamily> FriendsAndFamilies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Visitor> Visitors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HouseList> HouseLists { get; set; }
    }
}
