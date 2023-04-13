﻿using System;
using System.Collections.Generic;

namespace SneakerOnlineShop.Models
{
    public partial class Account
    {
        public int AccountId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int? RoleId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Role? Role { get; set; }
    }
}