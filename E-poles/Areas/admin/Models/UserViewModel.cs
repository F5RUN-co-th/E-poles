﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_poles.Areas.admin.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; } = true;
        public string UserId { get; set; }
        public string SelectedGroup { get; set; }
        public List<SelectListItem> GroupsList { get; set; }
        public string SelectedRole { get; set; }
    }
}
