﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;

namespace EcommerceV2.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoDAO _produtoDAO;
        private readonly CategoriaDAO _categoriaDAO;

        public ProdutoController(ProdutoDAO produtoDAO, CategoriaDAO categoriaDAO)
        {
            _produtoDAO = produtoDAO;
            _categoriaDAO = categoriaDAO;
        }
        public IActionResult Index()
        {
            ViewBag.DataHora = DateTime.Now;
            return View(_produtoDAO.ListarTodos());
        }

        public IActionResult Cadastrar()
        {
            ViewBag.Categorias =
                new SelectList(_categoriaDAO.ListarTodos(), "CategoriaId", "Nome");
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto p)
        {
            if (ModelState.IsValid)
            {
                if (_produtoDAO.Cadastrar(p))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Esse produto já existe!");
            }
            return View(p);
            
           
        }

        public IActionResult Remover(int id)
        {
            _produtoDAO.Remover(id);
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            return View(_produtoDAO.BuscarPorId(id));
        }

        [HttpPost]
        public IActionResult Alterar(Produto p)
        {
            _produtoDAO.Editar(p);
            return RedirectToAction("Index");
        }
    }
}