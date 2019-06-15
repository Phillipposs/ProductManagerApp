using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.DTOs
{
    class CategoryPostDTO
    {
        [JsonProperty("category")]
        public CategoryDTO category { get; set; }
    }
}
