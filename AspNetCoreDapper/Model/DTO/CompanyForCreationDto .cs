﻿namespace AspNetCoreDapper.Model.DTO
{
    public class CompanyForCreationDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Country { get; set; } = null!;
    }
}