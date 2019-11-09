using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EcommerceV2.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cadastrar()
        {
            Usuario u = new Usuario();
            if(TempData["Usuario"] != null)
            {
                string resultado = TempData["Usuario"].ToString();
                u.Endereco = JsonConvert.DeserializeObject<Endereco>(resultado);
            }            
            return View(u);
        }

        [HttpPost]
        public IActionResult BuscarCep(Usuario u)
        {
            string url = $"https://viacep.com.br/ws/{u.Endereco.Cep}/json/";
            WebClient client = new WebClient();
            TempData["Usuario"] = client.DownloadString(url);

            return RedirectToAction("Cadastrar");
        }
    }
}