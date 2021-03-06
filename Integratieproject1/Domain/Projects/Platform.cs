﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Projects
{
    public class Platform
    {
      [Key]
        public int PlatformId { get; set; }
    [Required]
        public string PlatformName { get; set; }
        public string Logo { get; set; }
        public Address Address { get; set; }
        public string Phonenumber { get; set; }
        public string Description { get; set; }
        public string BackgroundImage { get; set; }
        public string BackgroundColor { get; set; }
        public string ButtonColor { get; set; }
        public string TextColor { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<CustomUser> Users { get; set; }
    }
}
