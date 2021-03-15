using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalUI1.Models.ViewModels
{
    public class OneForAll
    {
        public  IEnumerable<Resident> resident { get; set; }
        public IEnumerable<Employee> employee { get; set; }
        public IEnumerable<Visitor> visitor { get; set; }
        public IEnumerable<Payment> payment { get; set; }
        public IEnumerable<Service> service { get; set; }
        public IEnumerable<DashboardPost> DashboardPost { get; set; }
        public IEnumerable<FriendsAndFamily> friendsAndFamily { get; set; }
        public IEnumerable<HouseList> houseList { get; set; }
        public IEnumerable<Complaint> complaint { get; set; }


    }
}