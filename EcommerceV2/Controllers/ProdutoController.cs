using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceV2.DAL;
using EcommerceV2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceV2.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDAO _produtoDAO;

        public ProdutoController(ProdutoDAO produtoDAO)
        {
            _produtoDAO = produtoDAO;
        }
        public IActionResult Index()
        {
            ViewBag.DataHora = DateTime.Now;
            return View(_produtoDAO.Listar());
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto p)
        {
            if(_produtoDAO.Cadastrar(p) == true)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Remover(int id)
        {
            _produtoDAO.Remover(id);
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            return View(_produtoDAO.BuscarProdutoPorId(id));
        }

        [HttpPost]
        public IActionResult Alterar(Produto p)
        {
            _produtoDAO.Editar(p);
            return RedirectToAction("Index");
        }
    }
}