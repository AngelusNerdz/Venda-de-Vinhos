using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UbotsTest.Models;

namespace UbotsTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private HttpClient httpClient = new HttpClient();
        private List<Cliente> clientes = new List<Cliente>();
        private List<Compra> compras = new List<Compra>();

        public ClientController ()
        {
            httpClient.BaseAddress = new Uri("http://www.mocky.io/v2/");
        }

        public void GetClientesEComprasDaApi()
        {
            var clientesHttpClientResponseStream = httpClient.GetStreamAsync("598b16291100004705515ec5");
            var comprasHttpClientResponseStream = httpClient.GetStreamAsync("598b16861100004905515ec7");

            clientes = JsonSerializer.DeserializeAsync<List<Cliente>>(clientesHttpClientResponseStream.Result).Result;
            compras = JsonSerializer.DeserializeAsync<List<Compra>>(comprasHttpClientResponseStream.Result).Result;
        }

        [HttpGet]
        public IEnumerable<Cliente> GetClientesOrdenadosPorTotalCompras()
        {
            GetClientesEComprasDaApi();

            return clientes.OrderByDescending(cliente => compras.Where(compra => VerificarIgualdadeCpfClienteEmCompras(compra.Cliente, cliente.Cpf))
                .Sum(compra => compra.ValorTotal))
                .ToList();
        }

        [HttpGet("maior-compra-2016")]
        public Cliente GetClienteMaiorCompra2016()
        {
            GetClientesEComprasDaApi();

            return clientes.FirstOrDefault(cliente => VerificarIgualdadeCpfClienteEmCompras(compras.FirstOrDefault(compra => compra.ValorTotal == compras.Max(compra => compra.ValorTotal) && compra.Data.Contains("2016")).Cliente, cliente.Cpf));
        }

        [HttpGet("mais-fieis")]
        public IEnumerable<Cliente> GetClientesMaisFieis()
        {
            int limiteComprasClienteNaoFiel = 2;

            GetClientesEComprasDaApi();

            return clientes.Where(cliente => compras.FindAll(compra => VerificarIgualdadeCpfClienteEmCompras(compra.Cliente, cliente.Cpf)).Count > limiteComprasClienteNaoFiel)
                .ToList();
        }

        [HttpGet("{idCliente}/vinho-recomendado")]
        public async Task<ActionResult<Produto>> GetVinhoMaisComprado(int idCliente)
        {
            GetClientesEComprasDaApi();

            var cliente = clientes.Find(cliente => cliente.Id == idCliente);

            if (cliente == null)
                return NotFound();

            return compras.Where(compra => VerificarIgualdadeCpfClienteEmCompras(compra.Cliente, cliente.Cpf))
                .SelectMany(compra => compra.Itens)
                .ToList()
                .GroupBy(produto => produto)
                .OrderByDescending(produto => produto.Count())
                .Select(produto => produto.Key)
                .First();
        }

        public bool VerificarIgualdadeCpfClienteEmCompras(string cpfClienteCompras, string cpfCliente)
        {
            if (String.IsNullOrEmpty(cpfCliente) || String.IsNullOrEmpty(cpfClienteCompras))
                return false;

            return cpfClienteCompras.Remove(12, 1).Insert(12, "-").Contains(cpfCliente);
        }
    }
}
