using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Fatec_Facilities.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly Contexto _context;
        
        public EmpresaController(Contexto context)
        {
            _context = context;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        // GET: Empresa
        public async Task<IActionResult> Index()
        {
            var contexto = _context.Empresa.Include(e => e.Endereco);
            return View(await contexto.ToListAsync());
        }

        [Authorize(Roles = "Gestor,Usuario")]
        // GET: Empresa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        [Authorize(Roles = "Gestor")]
        // GET: Empresa/Create
        public IActionResult Create()
        {
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "Logradouro");
            return View();
        }


        // POST: Empresa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome,NomeFantasia,Cnpj,EnderecoId")] Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "Logradouro", empresa.EnderecoId);
            return View(empresa);
        }

        // GET: Empresa/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "Id", empresa.EnderecoId);
            return View(empresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,NomeFantasia,Cnpj,EnderecoId")] Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empresa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpresaExists(empresa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnderecoId"] = new SelectList(_context.Endereco, "Id", "Id", empresa.EnderecoId);
            return View(empresa);
        }

        // GET: Empresa/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresa
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empresa = await _context.Empresa.FindAsync(id);
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool EmpresaExists(int id)
        {
            return _context.Empresa.Any(e => e.Id == id);
        }
        
        public Empresa BuscarEmpresaUsuarioLogado(HttpContext httpContext)
        {
            ClaimsPrincipal claimsPrincipal = httpContext.User as ClaimsPrincipal;

            var identities = (claimsPrincipal.Identities.ToList());

            var claims = identities[0].Claims.ToList();

            //----------------------------------------------------------------------------//

            UsuarioController _usuarioController = new UsuarioController(_context);

            Usuario gestor = _usuarioController.VerificaColaborador(claims[1].Value);

            UsuarioEmpresa usuarioEmpresa = _context.UsuarioEmpresa.Where(e => e.UsuarioId == gestor.Id).FirstOrDefault();

            Empresa empresa = _context.Empresa.Where(e => e.Id == usuarioEmpresa.EmpresaId ).FirstOrDefault();

            return empresa;
        }

        public Empresa RetornaEmpresaUsuario(Usuario usuario)
        {
            Empresa empresa = _context.Empresa
                                .Join(_context.UsuarioEmpresa,
                                        empresa => empresa.Id,
                                        usuarioEmpresa => usuarioEmpresa.EmpresaId,
                                        (empresa, usuarioEmpresa) => new {  Empresa = empresa, 
                                                                            UsuarioEmpresa = usuarioEmpresa})
                                .Where(e => e.UsuarioEmpresa.UsuarioId == usuario.Id)
                                .Select(e => e.Empresa)
                                .FirstOrDefault();
        
            return empresa;
        }

        public void setEmpresaUsuarioLogado(Empresa empresa)
        {
            Globals.EmpresaUsuarioLogado = empresa;
        }

        public Empresa getEmpresaUsuarioLogado()
        {
            return Globals.EmpresaUsuarioLogado;
        }
        
    }
}
