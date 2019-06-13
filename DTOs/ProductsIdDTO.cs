using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.DTOs
{
    class ProductsIdDTO
    {
        [JsonProperty("id")]
        public int id { get; set; }
    }
}
