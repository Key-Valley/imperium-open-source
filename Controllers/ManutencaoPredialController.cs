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
    public class ManutencaoPredialController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<ManutencaoPredialController> _logger;

        public ManutencaoPredialController(Contexto context, ILogger<ManutencaoPredialController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: ManutencaoPredial
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a pagina de Index");

                var contexto = _context.ManutencaoPredialEmpresa
                                    .Where(m => m.EmpresaId == EmpresaUsuario.Id)
                                    .Include(m => m.ManutencaoPredial)
                                    .Include(m => m.ManutencaoPredial.Fornecedor)
                                    .Include(m => m.ManutencaoPredial.Local)
                                    .Include(m => m.ManutencaoPredial.Status)
                                    .Select(m => m.ManutencaoPredial);
                
                return View(await contexto.ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: ManutencaoPredial/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manutencaoPredial = await _context.ManutencaoPredial
                .Include(m => m.Fornecedor)
                .Include(m => m.Local)
                .Include(m => m.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manutencaoPredial == null)
            {
                return NotFound();
            }

            return View(manutencaoPredial);
        }

        // GET: ManutencaoPredial/Create
        public IActionResult Create()
        {
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "Nome");
            ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome");
            ViewData["CategoriaManutencaoPredialId"] = new SelectList(_context.CategoriaManutencaoPredial, "Id", "Nome");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao");
            return View();
        }

        // POST: ManutencaoPredial/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao,LocalId,CategoriaManutencaoPredialId,DataSolicitacao,DataAprovacao,DataConclusao,StatusId,FornecedorId,Valor")] ManutencaoPredial manutencaoPredial)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(manutencaoPredial);
                    await _context.SaveChangesAsync();
                                    
                    if(this.CreateTerciaria(manutencaoPredial, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "Nome", manutencaoPredial.FornecedorId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", manutencaoPredial.LocalId);
                ViewData["CategoriaManutencaoPredialId"] = new SelectList(_context.CategoriaManutencaoPredial, "Id", "Nome");
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao", manutencaoPredial.StatusId);

                return View(manutencaoPredial);
                
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(ManutencaoPredial manutencaoPredial, Empresa Empresa)
        {
            try {
                _logger.LogInformation("Criando registro na tabela terciaria de ManutencaoPredial: {ManutencaoPredial}",manutencaoPredial.Id);

                ManutencaoPredialEmpresa manutencaoPredialEmpresa = new ManutencaoPredialEmpresa();

                int id = _context.ManutencaoPredial.Select(b => b.Id).ToList().LastOrDefault();

                manutencaoPredialEmpresa.Id = id + 1;
                manutencaoPredialEmpresa.ManutencaoPredialId = manutencaoPredial.Id;
                manutencaoPredialEmpresa.EmpresaId = Empresa.Id;

                _context.Add(manutencaoPredialEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criando registro na tabela terciaria de ManutencaoPredial: {ManutencaoPredial}",manutencaoPredial.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: ManutencaoPredial/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var manutencaoPredial = await _context.ManutencaoPredial.FindAsync(id);
                if (manutencaoPredial == null)
                {
                    return NotFound();
                }
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "Nome", manutencaoPredial.FornecedorId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", manutencaoPredial.LocalId);
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao", manutencaoPredial.StatusId);
                return View(manutencaoPredial);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ManutencaoPredial/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,LocalId,CategoriaManutencaoPredialId,DataSolicitacao,DataAprovacao,DataConclusao,StatusId,FornecedorId,Valor")] ManutencaoPredial manutencaoPredial)
        {
            try {
                if (id != manutencaoPredial.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(manutencaoPredial);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ManutencaoPredialExists(manutencaoPredial.Id))
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
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "Nome", manutencaoPredial.FornecedorId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", manutencaoPredial.LocalId);
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao", manutencaoPredial.StatusId);
                return View(manutencaoPredial);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ManutencaoPredial/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var manutencaoPredial = await _context.ManutencaoPredial
                    .Include(m => m.Fornecedor)
                    .Include(m => m.Local)
                    .Include(m => m.Status)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (manutencaoPredial == null)
                {
                    return NotFound();
                }

                return View(manutencaoPredial);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ManutencaoPredial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var manutencaoPredial = await _context.ManutencaoPredial.FindAsync(id);

                if(this.DeleteTerciaria(manutencaoPredial, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando ManutencaoPredial: {ManutencaoPredial}", manutencaoPredial.Id);

                    _context.ManutencaoPredial.Remove(manutencaoPredial);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(ManutencaoPredial manutencaoPredial, Empresa empresa)
        {
            _logger.LogInformation("Deletando reigstro da tabela terciaria: {ManutencaoPredial}", manutencaoPredial.Id);

            try {
                ManutencaoPredialEmpresa manutencaoPredialEmpresa = _context.ManutencaoPredialEmpresa
                                                                        .FirstOrDefault(
                                                                                m => m.ManutencaoPredialId == manutencaoPredial.Id
                                                                                && m.EmpresaId == empresa.Id
                                                                                        );

                _context.ManutencaoPredialEmpresa.Remove(manutencaoPredialEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
               _logger.LogError(e.ToString());
            }

            return false;
        }

        private bool ManutencaoPredialExists(int id)
        {
            try {
                return _context.ManutencaoPredial.Any(e => e.Id == id);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }
    }
}
