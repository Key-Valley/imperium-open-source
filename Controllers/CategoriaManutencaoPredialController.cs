using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;
using Microsoft.Extensions.Logging;

namespace Fatec_Facilities.Controllers
{
    public class CategoriaManutencaoPredialController : Controller
    {
        private readonly Contexto _context;

        private EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<CategoriaManutencaoPredial> _logger;

        public CategoriaManutencaoPredialController(Contexto context,
                                                    ILogger<CategoriaManutencaoPredial> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: CategoriaManutencaoPredial
        public async Task<IActionResult> Index()
        {
            try {
                return View(await _context.CategoriaManutencaoPredialEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                    .Include(c => c.CategoriaManutencaoPredial)
                                    .Select(c => c.CategoriaManutencaoPredial)
                                    .ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaManutencaoPredial/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaManutencaoPredial = await _context.CategoriaManutencaoPredialEmpresa
                    .Where(c => c.EmpresaId == EmpresaUsuario.Id 
                            && c.CategoriaManutencaoPredialId == id)
                    .Include(c => c.CategoriaManutencaoPredial)
                    .Select(c => c.CategoriaManutencaoPredial)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (categoriaManutencaoPredial == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes da CategoriaManutencaoPredial: "+
                "{CategoriaManutencaoPredial}", categoriaManutencaoPredial.Nome);

                return View(categoriaManutencaoPredial);
            } catch (Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaManutencaoPredial/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaManutencaoPredial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
                                        [Bind("Id,Nome")] CategoriaManutencaoPredial categoriaManutencaoPredial)
        {

            try {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Criando registro de CategoriaManutencaoPredial: "+
                                            "{CategoriaManutencaoPredial}"+ categoriaManutencaoPredial.Nome);                    

                    _context.Add(categoriaManutencaoPredial);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Criado registro de CategoriaManutencaoPredial: "+
                                            "{CategoriaContaMensal}",categoriaManutencaoPredial.Nome);

                    if(this.CreateTerciaria(categoriaManutencaoPredial, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    return View(categoriaManutencaoPredial);
                } 

            } catch (Exception e) {
                    _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria( CategoriaManutencaoPredial categoriaManutencaoPredial,
                                     Empresa empresa)
        {
            try{
                _logger.LogInformation("Criando registro CategoriaManutencaoPredial na terciaria: "+
                "{CategoriaManutencaoPredial}", categoriaManutencaoPredial.Nome);

                int id = _context.CategoriaManutencaoPredialEmpresa
                                    .Select(c => c.Id)
                                    .ToList()
                                    .LastOrDefault();

                CategoriaManutencaoPredialEmpresa categoriaManutencaoPredialEmpresa;
                categoriaManutencaoPredialEmpresa = new CategoriaManutencaoPredialEmpresa
                { 
                    Id = id + 1,
                    CategoriaManutencaoPredialId = categoriaManutencaoPredial.Id, 
                    EmpresaId = empresa.Id
                };

                _context.Add(categoriaManutencaoPredialEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro CategoriaManutencaoPredial  na terciaria: "+
                "{CategoriaManutencaoPredial}", categoriaManutencaoPredial.Nome);

                return true;
            } catch (Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }                           

        // GET: CategoriaManutencaoPredial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaManutencaoPredial = await _context.CategoriaManutencaoPredialEmpresa
                                                            .Where(
                                                                c => c.EmpresaId == EmpresaUsuario.Id
                                                                && c.CategoriaManutencaoPredialId == id
                                                            )
                                                            .Include(c => c.CategoriaManutencaoPredial)
                                                            .Select(c => c.CategoriaManutencaoPredial)
                                                            .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo edicao de CategoriaManutencaoPredial: "+
                                        "{CategoriaManutencaoPredial}", categoriaManutencaoPredial.Nome);
                
                if (categoriaManutencaoPredial == null)
                {
                    return NotFound();
                }
                return View(categoriaManutencaoPredial);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CategoriaManutencaoPredial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] CategoriaManutencaoPredial categoriaManutencaoPredial)
        {
            try
            {
                 if (id != categoriaManutencaoPredial.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(categoriaManutencaoPredial);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoriaManutencaoPredialExists(categoriaManutencaoPredial.Id))
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

                return View(categoriaManutencaoPredial);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaManutencaoPredial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaManutencaoPredial = await _context.CategoriaManutencaoPredialEmpresa
                                                                .Where(c => c.EmpresaId == EmpresaUsuario.Id
                                                                            && c.CategoriaManutencaoPredialId == id)
                                                                .Include(c => c.CategoriaManutencaoPredial)
                                                                .Select(c => c.CategoriaManutencaoPredial)
                                                                .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete de CategoriaManutencaoPredial: "+
                                        "{CategoriaManutencaoPredial}", categoriaManutencaoPredial.Nome);

                if (categoriaManutencaoPredial == null)
                {
                    return NotFound();
                }

                return View(categoriaManutencaoPredial);
            }catch (Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        } 

        // POST: CategoriaManutencaoPredial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var categoriaManutencaoPredial = await _context.CategoriaManutencaoPredial.FindAsync(id);

                if(this.DeletaTerciaria(categoriaManutencaoPredial, EmpresaUsuario))
                {
                    _context.CategoriaManutencaoPredial.Remove(categoriaManutencaoPredial);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));    
                }
            } catch (Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }
        
        public bool DeletaTerciaria(CategoriaManutencaoPredial categoriaManutencaoPredial, Empresa empresa)
        {
            try {
                CategoriaManutencaoPredialEmpresa categoriaManutencaoPredialEmpresa = _context
                                                            .CategoriaManutencaoPredialEmpresa
                                                            .FirstOrDefault(c => 
                                                            c.CategoriaManutencaoPredialId
                                                                ==
                                                            categoriaManutencaoPredial.Id
                                                            && c.EmpresaId == empresa.Id);

                _context.CategoriaManutencaoPredialEmpresa.Remove(categoriaManutencaoPredialEmpresa);

                _context.SaveChanges();

                return true;
            } catch (Exception e) {
                _logger.LogError(e.ToString());

                return false;
            }
        }

        private bool CategoriaManutencaoPredialExists(int id)
        {
            return _context.CategoriaManutencaoPredial.Any(e => e.Id == id);
        }
    }
}
