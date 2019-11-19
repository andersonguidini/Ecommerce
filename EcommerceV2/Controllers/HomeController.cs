using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using EcommerceV2.Utils;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace EcommerceV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProdutoDAO _produtoDAO;
        private readonly CategoriaDAO _categoriaDAO;
        private readonly ItemVendaDAO _itemVendaDAO;
        private readonly UtilsSession _utilsSession;
        public HomeController(ProdutoDAO produtoDAO, CategoriaDAO categoriaDAO, ItemVendaDAO itemVendaDAO, UtilsSession utilsSession)
        {
            _produtoDAO = produtoDAO;
            _categoriaDAO = categoriaDAO;
            _itemVendaDAO = itemVendaDAO;
            _utilsSession = utilsSession;
        }

        public IActionResult Index(int? id)
        {
            ViewBag.Categorias = _categoriaDAO.ListarTodos();
            if (id == null)
            {
                return View(_produtoDAO.ListarTodos());
            }
            return View(_produtoDAO.ListarPorCategoria(id));
        }

        public IActionResult Detalhes(int id)
        {
            return View(_produtoDAO.BuscarPorId(id));
        }

        public IActionResult AdicionarAoCarrinho(int id)
        {
            Produto p = _produtoDAO.BuscarPorId(id);
            ItemVenda item = new ItemVenda
            {
                Produto = p,
                Quantidade = 1,
                Preco = p.preco.Value,
                CarrinhoId = _utilsSession.RetornarCarrinhoId()
            };

            _itemVendaDAO.Cadastrar(item);
            return RedirectToAction("CarrinhoCompras");
        }

        public IActionResult CarrinhoCompras()
        {
            string carrinhoId = _utilsSession.RetornarCarrinhoId();
            return View(_itemVendaDAO.ListarPorCarrinhoId(carrinhoId));
        }
    }
}