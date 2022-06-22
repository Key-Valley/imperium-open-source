using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Fatec_Facilities.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Fatec_Facilities.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly Contexto _context;

        EmpresaController _empresaController;

        private Empresa EmpresaUsuarioLogado;

        public UsuarioController(Contexto context)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuarioLogado = _empresaController.BuscarEmpresaUsuarioLogado(HttpContext);
        }

        [Authorize]
        public IActionResult Home()
        {
            return View();
        }

        // GET: Gestors
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Empresa = EmpresaUsuarioLogado;

            Empresa emp = EmpresaUsuarioLogado;

            var contexto = _context.UsuarioEmpresa
                                .Where(u => u.EmpresaId == emp.Id)
                                .Include(e => e.Usuario)
                                .ToListAsync();

            return View(await contexto);
        }

        // GET: Gestors/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            EmpresaController _empresaController = new EmpresaController(_context);
            Empresa empresa = _empresaController.BuscarEmpresaUsuarioLogado(HttpContext);

            ViewBag.Empresa = empresa;

            var contexto = await _context.Usuarios.Include(e => e.UsuarioEmpresa).FirstOrDefaultAsync();

            if (contexto == null)
            {
                return NotFound();
            }

            return View(contexto);
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> EditProfile()
        {
            Usuario logado = GetUsuarioLogado(HttpContext);

            var usuarioLogado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == logado.Email);
            int id = usuarioLogado.Id;

            return RedirectToAction("Edit", new { id });
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> EditPassword(string NovaSenha, [Bind("Senha")] Usuario usuario)
        {
            Usuario logado = GetUsuarioLogado(HttpContext);

            var usuarioLogado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == logado.Email);


            if (usuario == null)
            {
                return NotFound();
            }

            if ((BC.Verify(usuario.Senha, usuarioLogado.Senha)))
            {
                usuarioLogado.Senha = BC.HashPassword(NovaSenha);
                _context.Update(usuarioLogado);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            EmpresaController _empresaController = new EmpresaController(_context);
            Empresa empresa = _empresaController.BuscarEmpresaUsuarioLogado(HttpContext);

            ViewBag.Empresa = empresa;

            return View();
        }

        // POST: Gestors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Rm,Gestor")] Usuario usuario)
        {
            Empresa empresa = _empresaController.BuscarEmpresaUsuarioLogado(HttpContext);

            usuario.Gestor = false;

            if (ModelState.IsValid)
            {
                usuario.Senha = BC.HashPassword(usuario.Senha);
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                await this.CreateTerciaria(usuario, empresa);

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        public async Task CreateTerciaria(Usuario usuario, Empresa empresa)
        {
            UsuarioEmpresa usuarioEmpresa = new UsuarioEmpresa();

            int id = _context.UsuarioEmpresa.Select(b => b.Id).ToList().LastOrDefault();

            usuarioEmpresa.Id = (id) + 1;
            usuarioEmpresa.UsuarioId = usuario.Id;
            usuarioEmpresa.EmpresaId = empresa.Id;

            _context.Add(usuarioEmpresa);

            await _context.SaveChangesAsync();
        }

        // GET: Gestors/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            ViewData["EmpresaId"] = new SelectList(_context.Empresa, "Id", "Nome");

            return View(usuario);
        }

        // POST: Gestors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Senha,Email,Rm,Gestor,EmpresaId")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            // if (ModelState.IsValid)
            // {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GestorExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["EmpresaId"] = new SelectList(_context.Empresa, "Id", "Nome");

            return RedirectToAction(nameof(Index));
        }

        // GET: Gestors/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Gestors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuario);

            UsuarioEmpresaController _usuarioEmpresaController = new UsuarioEmpresaController(_context);
            await _usuarioEmpresaController.Delete(id);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool GestorExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public string VerificaEmailColaborador(Usuario usuario)
        {
            Usuario verificarUsuarioEmail = _context.Usuarios.Where(u => u.Email == usuario.Email).FirstOrDefault();

            return verificarUsuarioEmail.Email;
        }

        public Usuario VerificaColaborador(string email)
        {
            Usuario usuario = _context.Usuarios.Where(u => u.Email == email).FirstOrDefault();

            return usuario;
        }

        public Usuario GetUsuarioLogado(HttpContext httpContext)
        {
            ClaimsPrincipal claimsPrincipal = httpContext.User as ClaimsPrincipal;

            var identities = (claimsPrincipal.Identities.ToList());

            var claims = identities[0].Claims.ToList();

            Usuario usuario = VerificaColaborador(claims[1].Value);

            return usuario;
        }
    }
}
