using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Fatec_Facilities.Controllers
{
    public class ProblematicasController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<ProblematicasController> _logger;

        public ProblematicasController(Contexto context, ILogger<ProblematicasController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: Problematicas
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {
                var contexto = _context.ProblematicaEmpresa
                                    .Where(p => p.EmpresaId == EmpresaUsuario.Id)
                                    .Include(p => p.Problematica)
                                    .Include(p => p.Problematica.Categoria)
                                    .Select(p => p.Problematica);

                return View(await contexto.ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Problematicas/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var problematica = await _context.ProblematicaEmpresa
                                    .Where(p => p.EmpresaId == EmpresaUsuario.Id && p.ProblematicaId == id)
                                    .Include(p => p.Problematica)
                                    .Include(p => p.Problematica.Categoria)
                                    .Select(p => p.Problematica)
                                    .FirstOrDefaultAsync();

                if (problematica == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes da Problematica", problematica.Id);

                return View(problematica);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Problematicas/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            _logger.LogInformation("Abrindo pagina de criacao de Problematica");

            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome");

            return View();
        }

        // POST: Problematicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Descricao,CategoriaId")] Problematica problematica)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(problematica);
                    await _context.SaveChangesAsync();
                    if(this.CreateTerciaria(problematica, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", problematica.CategoriaId);

                return View(problematica);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(Problematica problematica, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro da Problematica na tabela terciaria: {Problematica}", problematica.Id);

                ProblematicaEmpresa problematicaEmpresa = new ProblematicaEmpresa();

                int id = _context.ProblematicaEmpresa.Select(p => p.Id).ToList().LastOrDefault();

                problematicaEmpresa.Id = id + 1;
                problematicaEmpresa.ProblematicaId = problematica.Id;
                problematicaEmpresa.EmpresaId = empresa.Id;

                _context.Add(problematicaEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro da Problematica na tabela terciaria: {Problematica}", problematica.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: Problematicas/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var problematica = await _context.ProblematicaEmpresa
                                    .Where(p => p.EmpresaId == EmpresaUsuario.Id && p.ProblematicaId == id)
                                    .Include(p => p.Problematica)
                                    .Include(p => p.Problematica.Categoria)
                                    .Select(p => p.Problematica)
                                    .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo edicao da Problematica: {Problematica}", problematica.Id);

                if (problematica == null)
                {
                    return NotFound();
                }

                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", problematica.CategoriaId);

                return View(problematica);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Problematicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,CategoriaId")] Problematica problematica)
        {
            if (id != problematica.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(problematica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProblematicaExists(problematica.Id))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", problematica.CategoriaId);
            return View(problematica);
        }

        // GET: Problematicas/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var problematica = await _context.ProblematicaEmpresa
                                    .Where(p => p.EmpresaId == EmpresaUsuario.Id && p.ProblematicaId == id)
                                    .Include(p => p.Problematica)
                                    .Include(p => p.Problematica.Categoria)
                                    .Select(p => p.Problematica)
                                    .FirstOrDefaultAsync();

            if (problematica == null)
            {
                return NotFound();
            }

            return View(problematica);
        }

        // POST: Problematicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var problematica = await _context.ProblematicaEmpresa
                                    .Where(p => p.EmpresaId == EmpresaUsuario.Id && p.ProblematicaId == id)
                                    .Include(p => p.Problematica)
                                    .Include(p => p.Problematica.Categoria)
                                    .Select(p => p.Problematica)
                                    .FirstOrDefaultAsync();

                if(this.DeletaTerciaria(problematica, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando Problematica: {Problematica}", problematica.Id);

                    _context.Problematica.Remove(problematica);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado Problematica: {Problematica}", problematica.Id);

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeletaTerciaria(Problematica problematica, Empresa empresa)
        {
            _logger.LogInformation("Deletando registro na tabela terciaria: {Problematica}", problematica.Id);

            try {
                ProblematicaEmpresa problematicaEmpresa = _context.ProblematicaEmpresa
                                                            .FirstOrDefault(p => p.ProblematicaId == problematica.Id
                                                                            && p.EmpresaId == empresa.Id);

                _context.ProblematicaEmpresa.Remove(problematicaEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool ProblematicaExists(int id)
        {
            try {
                return _context.Problematica.Any(e => e.Id == id);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return false;
        }
    }
}
