using Newtonsoft.Json;
using NopCommerce.Api.SampleApplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.DTOs
{
    public class CustomerSendDTO
    {
        [JsonProperty("customer")]
        public CustomerCreateDTO customer { get; set; }

       
    }
}
