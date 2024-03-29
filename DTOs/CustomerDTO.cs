﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace NopCommerce.Api.SampleApplication.DTOs
{
    // Simplified Customer dto object with only the first and last name
    public class CustomerDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }



        [JsonProperty("role_ids")]
        public List<int> CustomerRoles { get; set; } = new List<int>();
    }
}