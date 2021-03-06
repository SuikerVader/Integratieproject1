﻿using System.ComponentModel.DataAnnotations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Location
    {
        [Key] public int LocationId { get; set; }
        public string LocationName { get; set; }
        public Address Address { get; set; }
        public Position Position { get; set; }
    }
}