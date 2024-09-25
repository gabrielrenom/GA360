﻿using GA360.DAL.Entities.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GA360.DAL.Entities.Enums.StatusEnum;

namespace GA360.Domain.Core.Models
{
    public class CustomerModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public string FatherName { get; set; }
        public string About { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? Description { get; set; }
        public string Location { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public Guid? TenantId { get; set; }
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string DOB { get; set; }
        public string NI { get; set; }
        public string Disability { get; set; }
        public string EmploymentStatus { get; set; }
        public string Employer { get; set; }
        public string ePortfolio { get; set; }
        public int EthnicOriginId { get; set; }
        public string EthnicOrigin { get; set; }
        public string AvatarImage { get; set; }
        public string Portfolio { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Postcode { get; set; }
        public string DateOfBirth { get; set; }
        public string Ethnicity { get; set; }
        public string EmployeeStatus { get; set; }
        public int Status { get; set; }
        public int? TrainingCentre { get; set; }
        public string NationalInsurance { get; set; }
        public string[] Skills { get;set; }
    }
}