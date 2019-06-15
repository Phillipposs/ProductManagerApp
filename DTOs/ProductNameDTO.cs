using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.DTOs
{
    public class ProductNameDTO
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
