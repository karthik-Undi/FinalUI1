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
    
    public partial class DashboardPost
    {
        public DashboardPost(int postPostedBy, string postTitle, string postType, string postIntendedFor, string postDescription, DateTime? postTime)
        {
            this.PostPostedBy = postPostedBy;
            this.PostTitle = postTitle;
            this.PostType = postType;
            this.PostIntendedFor = postIntendedFor;
            this.PostDescription = postDescription;
            this.PostTime = postTime;
        }
        public DashboardPost()
        {

        }

        public int PostID { get; set; }
        public int PostPostedBy { get; set; }
        public string PostTitle { get; set; }
        public string PostType { get; set; }
        public string PostDescription { get; set; }
        public string PostIntendedFor { get; set; }
        public Nullable<System.DateTime> PostTime { get; set; }
    
        public virtual Resident Resident { get; set; }
    }
}
