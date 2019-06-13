using System.Collections.Generic;
using Newtonsoft.Json;

namespace NopCommerce.Api.SampleApplication.DTOs
{
    public class CustomersRootObject
    {
        [JsonProperty("customers")]
        public List<CustomerDTO> Customers { get; set; }
    }
}