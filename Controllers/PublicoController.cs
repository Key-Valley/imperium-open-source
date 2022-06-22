using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fatec_Facilities.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Web;
using BC = BCrypt.Net.BCrypt;

namespace Fatec_Facilities.Controllers
{
    public class PublicoController : Controller
    {
        private readonly Contexto _contexto;
        EmpresaController _empresaController;

        public PublicoController(Contexto contexto)
        {
            _contexto = contexto;

            _empresaController = new EmpresaController(_contexto);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logar()
        {
            return View();
        }

        public IActionResult AcessoNegado()
        {
            return View();
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Logout()
        {
            /*
            CookieOptions cookie = new CookieOptions();
            Response.Cookies.Delete("Email");
            return RedirectToAction("Logar", "Publico");
            */

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Logar", "Publico");
        }

        [HttpPost]

        public IActionResult Logar(string email, string senha)
        {
            Usuario usuario = AutenticarUser(email, senha);

            if(usuario == null || usuario.Equals(null))
            {
                ModelState.AddModelError(string.Empty, "O usuário não existe ou não foi encontrado, verifique o e-mail e a senha");
                return View();
            }    

            if (usuario.Email == null || usuario.Email == "")
            {
                ViewBag.Error = "E-mail e/ou senha incorreto(s)";
                return View();
            }
            else
            {
                Empresa empresa = _empresaController.RetornaEmpresaUsuario(usuario);

                if (usuario.Gestor == false)
                {
                    return RedirectToAction("Mapa", "Blocoes", new { empresaId = empresa.Id});
                }
                else
                {
                    return RedirectToAction("Mapa", "Blocoes", new { empresaId = empresa.Id});
                }

            }
        }

        public Usuario AutenticarUser(string email, string senha)
        {

            Usuario usuario = _contexto.Usuarios.FirstOrDefault(g=>g.Email == email);

            if (usuario != null && usuario.Senha != null && BC.Verify(senha, usuario.Senha))
            {
                _ = CriarCookie(usuario);

                return usuario;
            }
            else
            {
                return null;
            }
        }

        public async Task CriarCookie(Usuario usuario)
        {
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                };

            if (usuario.Gestor.Equals(true))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Gestor"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Usuario"));
            }

            var userClaims = claims;

            var minhaIdentiy = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(minhaIdentiy);

            Empresa EmpresaUsuarioLogado = _empresaController.RetornaEmpresaUsuario(usuario);

            _empresaController.setEmpresaUsuarioLogado(EmpresaUsuarioLogado);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,userPrincipal);
        }

        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EsqueciSenha(String email)
        {
            int token = 0;

            EmailSenderController emailSender = new EmailSenderController(_contexto);

            if (ModelState.IsValid)
            { 
                Usuario user = _contexto.Usuarios.FirstOrDefault(g => g.Email == email);

                token = user.GetHashCode();

                var passwordResetLink = Url.Action("ResetarSenha", "Publico", new { email = email, token = token}, Request.Scheme);

                EmailSender emailWithPassword = new EmailSender();

                emailWithPassword.Destinatario = email;
                emailWithPassword.Titulo = "Recuperação de senha - Fatec Facilities";
                emailWithPassword.Corpo = "Por favor, acesse o link abaixo para poder resetar sua senha: \n" + passwordResetLink;

                emailSender.sendEmail(emailWithPassword);
            }

            SaveToken saveToken = new SaveToken();

            saveToken.Email = email;
            saveToken.Token = token;

            emailSender.SaveToken(saveToken);

            return View("EsqueciSenhaConfirmacao");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetarSenha(int token, string email)
        {
            // If password reset token or email is null, most likely the
            // user tried to tamper the password reset link
            if (token.Equals(null) || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ResetarSenha(string email, int token, string senha)
        {
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = _contexto.Usuarios.FirstOrDefault(g => g.Email == email);

                if (user != null)
                {
                    var validation = _contexto.SaveToken.FirstOrDefault(s => (s.Email == user.Email) && (s.Token == token));

                    if(validation != null)
                    {
                        user.Senha = BC.HashPassword(senha);
                        _contexto.SaveChanges();
                        return View("ResetarSenhaConfirmacao");
                    }
                }
            }
            return View("ResetarSenha");
        }

    }
}
