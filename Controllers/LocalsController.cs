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
    public class LocalsController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<LocalsController> _logger;

        public LocalsController(Contexto context, ILogger<LocalsController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public JsonResult LoadEquipaments(int? id)
        {
            try {

                var equipaments = _context.EquipamentoEmpresa
                    .Include(e => e.Equipamento)
                    .Where(b => b.Equipamento.BlocoId == id 
                            && b.EmpresaId == EmpresaUsuario.Id)
                    .GroupBy(e => e.Equipamento.LocalId)
                    .Select(g => new { id = g.Key, qtd = g.Count() })
                    .ToList();
                
                _logger.LogInformation("Realizando contagem dos equipamentos...");

                return Json(equipaments); 

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return Json(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Locais(int? id)
        {
            try {
                var locais = _context.LocalEmpresa
                    .Where(b => b.Local.BlocoID == id
                            && b.EmpresaId == EmpresaUsuario.Id)
                    .Include(l => l.Local)
                    .Select(b => b.Local)
                    .OrderBy(b => b.Nome)
                    .ToArrayAsync();

                _logger.LogInformation("Carregando os locais");

                if (id != null)
                {
                    var contexto = _context.Local.Include(l => l.Bloco);
                    return View(await locais);
                }
                else
                {
                    return NotFound();
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Locals
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {

                var contexto = _context.LocalEmpresa
                                    .Where(l => l.EmpresaId == EmpresaUsuario.Id)
                                    .Include(l => l.Local)
                                    .Include(l => l.Local.Bloco)
                                    .Select(l => l.Local);

                _logger.LogInformation("Carregando a pagina de Index");

                return View(await contexto.ToListAsync());

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Locals/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var local = await _context.LocalEmpresa
                    .Where(l => l.EmpresaId == EmpresaUsuario.Id
                            && l.LocalId == id)
                    .Include(l => l.Local)
                    .Include(l => l.Local.Bloco)
                    .Select(l => l.Local)
                    .FirstOrDefaultAsync();

                if (local == null)
                {
                    return NotFound();
                }

                return View(local);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Locals/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            try {
                ViewData["BlocoID"] = new SelectList(_context.Blocos, "Id", "Nome");
                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        // POST: Locals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome,BlocoID")] Local local)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(local);
                    await _context.SaveChangesAsync();

                    if(this.CreateTerciaria(local, EmpresaUsuario)) 
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["BlocoID"] = new SelectList(_context.Blocos, "Id", "Nome", local.BlocoID);
                return View(local);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        public bool CreateTerciaria(Local local, Empresa empresa) 
        {
            try {

                _logger.LogInformation("Criando registro do bloco na tabela terciaria: {Local}", local.Id);

                LocalEmpresa localEmpresa = new LocalEmpresa();

                int id = _context.LocalEmpresa.Select(l => l.Id).ToList().LastOrDefault();

                localEmpresa.Id = id + 1;
                localEmpresa.LocalId = local.Id;
                localEmpresa.EmpresaId = empresa.Id;

                _context.Add(localEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro do bloco na tabela terciaria: {Local}", local.Id);

                return true;

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: Locals/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var local = await _context.LocalEmpresa
                                    .Where(l => l.EmpresaId == EmpresaUsuario.Id
                                            && l.LocalId == id)
                                    .Include(l => l.Local)
                                    .Select(l => l.Local)
                                    .FirstOrDefaultAsync();

                if (local == null)
                {
                    return NotFound();
                }

                ViewData["BlocoID"] = new SelectList(_context.Blocos, "Id", "Nome", local.BlocoID);

                return View(local);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Locals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,BlocoID")] Local local)
        {
            try {
                if (id != local.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(local);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LocalExists(local.Id))
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
                ViewData["BlocoID"] = new SelectList(_context.Blocos, "Id", "Nome", local.BlocoID);
                return View(local);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            } 

            return RedirectToAction(nameof(Index));
        }

        // GET: Locals/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var local = await _context.LocalEmpresa
                    .Where(l => l.EmpresaId == EmpresaUsuario.Id 
                            && l.LocalId == id)
                    .Include(l => l.Local)
                    .Include(l => l.Local.Bloco)
                    .Select(l => l.Local)
                    .FirstOrDefaultAsync();

                if (local == null)
                {
                    return NotFound();
                }

                return View(local);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Locals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var local = await _context.Local.FindAsync(id);

                if(this.DeleteTerciaria(local, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando Local: {Local}", local.Id);

                    _context.Local.Remove(local);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado Local: {Local}", local.Id);
                }

                return RedirectToAction(nameof(Index));

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(Local local, Empresa empresa)
        {
            try {

                _logger.LogInformation("Deletando registro na tabela terciaria: {Local}", local.Id);

                LocalEmpresa localEmpresa = _context.LocalEmpresa
                                                .FirstOrDefault(l => l.LocalId == local.Id 
                                                                && l.EmpresaId == empresa.Id);

                _context.LocalEmpresa.Remove(localEmpresa);
                _context.SaveChanges();

                return true;

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool LocalExists(int id)
        {
            return _context.Local.Any(e => e.Id == id);
        }
    }
}
