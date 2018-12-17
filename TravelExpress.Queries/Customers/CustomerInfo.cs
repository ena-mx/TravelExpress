using System;

namespace TravelExpress.Queries.Customers
{
    public class CustomerInfo
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string FamilyName1 { get; set; }
        public string FamilyName2 { get; set; }
        public string Telephone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
    }
}
