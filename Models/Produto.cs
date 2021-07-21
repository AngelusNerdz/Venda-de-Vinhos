using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UbotsTest.Models
{
    public class Produto
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }
        
        [JsonPropertyName("produto")]
        public string Nome { get; set; }

        [JsonPropertyName("variedade")]
        public string Variedade { get; set; }

        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        [JsonPropertyName("categoria")]
        public string Categoria { get; set; }

        [JsonPropertyName("safra")]
        public string Safra { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }
    }
}
