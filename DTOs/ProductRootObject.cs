using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WpfTest.DTOs
{
    public class ProductsRootObject
    {
        [JsonProperty("products")]
        public List<ProductDTO> Products { get; set; }
    }
}
