using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTest.DTOs
{
    public class ProductDTO
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("short_description")]
        public string short_description { get; set; }

        [JsonProperty("full_description")]
        public string full_description { get; set; }

        [JsonProperty("show_on_home_page")]
        public string show_on_home_page { get; set; }

        [JsonProperty("sku")]
        public string sku { get; set; }

        [JsonProperty("price")]
        public string price { get; set; }

        [JsonProperty("weight")]
        public string weight { get; set; }

        [JsonProperty("length")]
        public string length { get; set; }

        [JsonProperty("height")]
        public string height { get; set; }

        [JsonProperty("width")]
        public string width { get; set; }

        [JsonProperty("stock_quantity")]
        public string stock_quantity { get; set; }

        [JsonProperty("old_price")]
        public string old_price { get; set; }

        [JsonProperty("special_price")]
        public string special_price { get; set; }

        [JsonProperty("special_price_start_date_time_utc")]
        public string special_price_start_date_time_utc { get; set; }

        [JsonProperty("special_price_end_date_time_utc")]
        public string special_price_end_date_time_utc { get; set; }
    }
}
