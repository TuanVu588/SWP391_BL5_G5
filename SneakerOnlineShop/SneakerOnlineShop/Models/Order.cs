using SneakerOnlineShop.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SneakerOnlineShop.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd-MM-yyyy}")]
        public DateTime? OrderDate { get; set; }
        //[DateGreaterThanNow(ErrorMessage = "The RequiredDate must be greater than current date and time.")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd-MM-yyyy}")]
        [Required(ErrorMessage = "RequiredDate is required")]
        public DateTime? RequiredDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd-MM-yyyy}")]
        public DateTime? ShippedDate { get; set; }
        public decimal? Freight { get; set; }
        [Required(ErrorMessage = "ShipAddress is required")]
        public string? ShipAddress { get; set; }
        [Required(ErrorMessage = "ShipAddress is required")]
        public string? ShipCity { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
