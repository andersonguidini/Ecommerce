using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository;

namespace EcommerceV2.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioDAO _usuarioDAO;
        private readonly UserManager<UsuarioLogado> _userManager;
        private readonly SignInManager<UsuarioLogado> _signInManager;

        public UsuarioController(UsuarioDAO usuarioDAO, UserManager<UsuarioLogado> userManager, SignInManager<UsuarioLogado> signInManager)
        {
            _usuarioDAO = usuarioDAO;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View(_usuarioDAO.ListarTodos());
        }

        public IActionResult Cadastrar()
        {
            Usuario u = new Usuario();
            if (TempData["Usuario"] != null)
            {
                string resultado = TempData["Usuario"].ToString();
                //Newtonsoft.Json
                u.Endereco = JsonConvert.
                    DeserializeObject<Endereco>(resultado);
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

        [HttpPost]
        public async Task<IActionResult> Cadastrar(Usuario u)
        {
            if (ModelState.IsValid)
            {
                UsuarioLogado uLogado = new UsuarioLogado
                {
                    UserName = u.Email,
                    Email = u.Email
                };
                //Cadastrar o user na tabela do Identity
                IdentityResult result = await _userManager.CreateAsync(uLogado, u.Senha);
                
                if (result.Succeeded)
                {
                    //Token confirmação email
                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(uLogado);

                    //Autenticação do usuário
                    await _signInManager.SignInAsync(uLogado, false);
                    if (_usuarioDAO.Cadastrar(u))
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Esse e-mail já está sendo usado!");
                }
                AdicionarErros(result);
               
            }
            return View(u);
        }

        private void AdicionarErros(IdentityResult result)
        {
            foreach (var erro in result.Errors)
            {
                ModelState.AddModelError("", erro.Description);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario u)
        {
            var result = await _signInManager.PasswordSignInAsync(u.Email, u.Senha, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Produto");
            }
            ModelState.AddModelError("", "Falha no login!");
            return View(u);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}