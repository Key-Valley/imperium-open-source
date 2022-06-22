using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Authorization;

namespace Fatec_Facilities.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly Contexto _context;

        private EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        public CategoriasController(Contexto context)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();
        }

        // GET: Categorias
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            return View(
                    await _context.CategoriaEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                    .Include(c => c.Categoria)
                                    .Select(c => c.Categoria)
                                    .ToListAsync()
                    );
        }

        // GET: Categorias/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.CategoriaEmpresa
                                        .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.CategoriaId == id)
                                        .Include(c => c.Categoria)
                                        .Select(c => c.Categoria)
                                        .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();

                Console.WriteLine(categoria);

                if(this.CreateTerciaria(categoria, EmpresaUsuario))
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(categoria);
        }

        public bool CreateTerciaria(Categoria categoria, Empresa empresa)
        {
            try{
                CategoriaEmpresa categoriaEmpresa = new CategoriaEmpresa();

                int id = _context.CategoriaEmpresa.Select(b => b.Id).ToList().LastOrDefault();

                categoriaEmpresa.Id = id + 1;
                categoriaEmpresa.CategoriaId = categoria.Id;
                categoriaEmpresa.EmpresaId = empresa.Id;

                _context.Add(categoriaEmpresa);
                _context.SaveChanges();

                Console.WriteLine(categoriaEmpresa);

                return true;
            } catch (Exception e) {
                return false;
            }
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.CategoriaEmpresa
                                            .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.CategoriaId == id)
                                            .Include(c => c.Categoria)
                                            .Select(c => c.Categoria)
                                            .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Categoria categoria)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        // GET: Categorias/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.CategoriaEmpresa
                                        .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.CategoriaId == id)
                                        .Include(c => c.Categoria)
                                        .Select(c => c.Categoria)
                                        .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            
            try{
                if(this.DeletaTerciaria(categoria, EmpresaUsuario))
                {
                    _context.Categoria.Remove(categoria);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            } catch (Exception e)
            {
                Console.WriteLine("DeleteConfirmed+:"+e);
            }
            
            return View(categoria);
        }

        public bool DeletaTerciaria(Categoria categoria, Empresa empresa)
        {
            try{
                CategoriaEmpresa categoriaEmpresa = _context
                                                        .CategoriaEmpresa
                                                        .Where(c => c.CategoriaId == categoria.Id
                                                                        && c.EmpresaId == empresa.Id)
                                                        .FirstOrDefault();

                _context.CategoriaEmpresa.Remove(categoriaEmpresa);
                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                return false;
            }
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }
    }
}
