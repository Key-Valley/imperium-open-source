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
    public class ContaMensalController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<ContaMensalController> _logger;

        public ContaMensalController(Contexto context, ILogger<ContaMensalController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: ContaMensal
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a página index");

                var contaMensalEmpresa = await _context.ContaMensalEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                    .Include(c => c.ContaMensal)
                                    .Select(c => c.ContaMensal)
                                    .ToListAsync();

                _logger.LogInformation("{ContaMensalEmprea}", contaMensalEmpresa);

                List<ContaMensal> contaMensal = new List<ContaMensal>();

                for(int i = 0; i < contaMensalEmpresa.Count(); i++)
                {
                    _logger.LogInformation("{Contador}",i);

                    contaMensal.Add(await _context.ContaMensal
                            .Where(c => c.Id == contaMensalEmpresa[i].Id)
                            .Include(c => c.CategoriaContaMensal)
                            .FirstOrDefaultAsync());
                }

                //_logger.LogInformation("Test: {CategoriaContaMensal}", contaMensal[0].CategoriaContaMensal.Descricao);

                return View(contaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ContaMensal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                } 

                var contaMensalEmpresa = await _context.ContaMensalEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                    .Include(c => c.ContaMensal)
                                    .Select(c => c.ContaMensal)
                                    .FirstOrDefaultAsync(m => m.Id == id);

                var contaMensal = await _context.ContaMensal
                                    .Where(c => c.Id == contaMensalEmpresa.Id)
                                    .Include(c => c.CategoriaContaMensal)
                                    .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de detalhes: {ContaMensal}", contaMensal.Id);

                if (contaMensal == null)
                {
                    return NotFound();
                }

                return View(contaMensal);
            } catch(Exception e) {
                    _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ContaMensal/Create
        public IActionResult Create()
        {
            try {
                _logger.LogInformation("Abrindo a pagina de criacao");
                ViewData["CategoriaContaMensalId"] = new SelectList(_context.CategoriaContaMensal, "Id", "Descricao");
                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: ContaMensal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoriaContaMensalId,Valor,DataVencimento,DataPagamento")] ContaMensal contaMensal)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(contaMensal);
                    await _context.SaveChangesAsync();

                    if(this.CreateTerciaria(contaMensal, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["CategoriaContaMensalId"] = new SelectList(_context.CategoriaContaMensal, "Id", "Descricao", contaMensal.CategoriaContaMensal.Descricao);
                return View(contaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(ContaMensal contaMensal, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro da ContaMensal na tabela terciaria: {ContaMensal}"+contaMensal.Id);

                ContaMensalEmpresa contaMensalEmpresa = new ContaMensalEmpresa();

                int id = _context.ContaMensalEmpresa.Select(c => c.Id).ToList().LastOrDefault();

                contaMensalEmpresa.Id = id + 1;
                contaMensalEmpresa.ContaMensalId = contaMensal.Id;
                contaMensalEmpresa.EmpresaId = empresa.Id;

                _context.Add(contaMensalEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criando registro da ContaMensal na tabela terciaria: {ContaMensal}"+contaMensal.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: ContaMensal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var contaMensalEmpresa = await _context.ContaMensalEmpresa
                                            .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.ContaMensalId == id)
                                            .Include(c => c.ContaMensal)
                                            .Select(c => c.ContaMensal)
                                            .FirstOrDefaultAsync();

                var contaMensal = await _context.ContaMensal
                                                .Where(c => c.Id == contaMensalEmpresa.Id)
                                                .Include(c => c.CategoriaContaMensal)
                                                .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a edicao da ContaMensal: {ContaMensal}", contaMensal.Id);

                if (contaMensal == null)
                {
                    return NotFound();
                }
                ViewData["CategoriaContaMensalId"] = new SelectList(_context.CategoriaContaMensal, "Id", "Descricao", contaMensal.CategoriaContaMensal.Descricao);
                return View(contaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ContaMensal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
        [Bind("Id,CategoriaContaMensalId,Valor,DataVencimento,DataPagamento")] ContaMensal contaMensal)
        {
            try {
                if (id != contaMensal.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _logger.LogInformation("Alterando registro da ContaMensal: {ContaMensal}", contaMensal.Id); 

                        _context.Update(contaMensal);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Alterado registro da ContaMensal: {ContaMensal}", contaMensal.Id);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ContaMensalExists(contaMensal.Id))
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
                ViewData["CategoriaContaMensalId"] = new SelectList(_context.CategoriaContaMensal, "Id", "Descricao", contaMensal.CategoriaContaMensal.Descricao);
                return View(contaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ContaMensal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try {

                _logger.LogInformation("Abrindo delete de ContaMensal");

                 if (id == null)
                {
                    return NotFound();
                }

                var contaMensalEmpresa = await _context.ContaMensalEmpresa
                                            .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.ContaMensalId == id)
                                            .Include(c => c.ContaMensal)
                                            .Select(c => c.ContaMensal)
                                            .FirstOrDefaultAsync();

                var contaMensal = await _context.ContaMensal
                                                .Where(c => c.Id == contaMensalEmpresa.Id)
                                                .Include(c => c.CategoriaContaMensal)
                                                .FirstOrDefaultAsync();

                if (contaMensal == null)
                {
                    return NotFound();
                }

                return View(contaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ContaMensal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var contaMensalEmpresa = await _context.ContaMensalEmpresa
                                                .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.ContaMensalId == id)
                                                .Include(c => c.ContaMensal)
                                                .Select(c => c.ContaMensal)
                                                .FirstOrDefaultAsync();

                var contaMensal = await _context.ContaMensal
                                                .Where(c => c.Id == contaMensalEmpresa.Id)
                                                .Include(c => c.CategoriaContaMensal)
                                                .FirstOrDefaultAsync();

            
                if(this.DeleteTerciaria(contaMensal, EmpresaUsuario))
                {
                    _context.ContaMensal.Remove(contaMensal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }                
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(ContaMensal contaMensal, Empresa empresa)
        {
            _logger.LogInformation("Deletando registro da tabela terciaria: {ContaMensal}",contaMensal.Id);

            try {
                ContaMensalEmpresa contaMensalEmpresa = _context
                                    .ContaMensalEmpresa
                                    .FirstOrDefault(c => c.ContaMensalId == contaMensal.Id
                                                        && c.EmpresaId == empresa.Id);

                _context.ContaMensalEmpresa.Remove(contaMensalEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogInformation(e.ToString());
            }

            return false;
        }

        private bool ContaMensalExists(int id)
        {
            return _context.ContaMensal.Any(e => e.Id == id);
        }
    }
}
