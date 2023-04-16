using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SneakerOnlineShop.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Orders = new HashSet<Order>();
        }

        public string CustomerId { get; set; } = null!;
        [Required(ErrorMessage = "Company name is required")]
        public string CompanyName { get; set; } = null!;
        [Required(ErrorMessage = "Contact name is required")]
        public string? ContactName { get; set; }
        [Required(ErrorMessage = "Contact title is required")]
        public string? ContactTitle { get; set; }
        [Required(ErrorMessage = "Address is required")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{4}$", ErrorMessage = "Please enter a valid phone number in the format xxx-xxx-xxxx")]
        public string? Phone { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
