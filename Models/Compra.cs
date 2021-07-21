using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UbotsTest.Models
{
    public class Compra
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("data")]
        public string Data { get; set; }

        [JsonPropertyName("cliente")]
        public string Cliente { get; set; }

        [JsonPropertyName("itens")]
        public List<Produto> Itens { get; set; }

        [JsonPropertyName("valorTotal")]
        public decimal ValorTotal { get; set; }
    }
}
