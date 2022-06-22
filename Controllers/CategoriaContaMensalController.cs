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
    public class CategoriaContaMensalController : Controller
    {
        private readonly Contexto _context;

        private EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<CategoriaContaMensalController> _logger;

        public CategoriaContaMensalController(  Contexto context, 
                                                ILogger<CategoriaContaMensalController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: CategoriaContaMensal
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a página de 'Categoria Conta Mensal'");
                
                return View(await _context.CategoriaContaMensalEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                    .Include(c => c.CategoriaContaMensal)
                                    .Select(c => c.CategoriaContaMensal)
                                    .ToListAsync());
            } catch(Exception e){
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaContaMensal/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaContaMensal = await _context.CategoriaContaMensalEmpresa
                                                    .Where  ( c => c.EmpresaId == EmpresaUsuario.Id
                                                                &&  c.CategoriaContaMensalId == id
                                                            )
                                                    .Include(c => c.CategoriaContaMensal)
                                                    .Select(c => c.CategoriaContaMensal)
                                                    .FirstOrDefaultAsync(m => m.Id == id);
                if (categoriaContaMensal == null)
                {
                    return NotFound();
                }

                _logger.LogInformation(
                        "Abrindo a pagina de detalhes da categoria conta mensal:" +
                        "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                return View(categoriaContaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriaContaMensal/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriaContaMensal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Descricao")] 
                                                CategoriaContaMensal categoriaContaMensal)
        {
            try{
                if(ModelState.IsValid)
                {
                    _logger.LogInformation( "Criando registro de categoriaContaMensal: "+ 
                                            "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                    _context.Add(categoriaContaMensal);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation( "Criado registro de categoriaContaMensal: "+ 
                                            "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                    if(this.CreateTerciaria(categoriaContaMensal, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                    return View(categoriaContaMensal);
                }
                
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(CategoriaContaMensal categoriaContaMensal, Empresa empresa)
        {
            try {
                _logger.LogInformation( "Criando registro da categoriaContaMensal na tabela terciaria: "+
                                        "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                int id = _context.CategoriaContaMensalEmpresa
                                    .Select(c => c.Id)
                                    .ToList()
                                    .LastOrDefault();

                CategoriaContaMensalEmpresa categoriaContaMensalEmpresa;
                categoriaContaMensalEmpresa = new CategoriaContaMensalEmpresa
                {
                    Id = id+1,
                    CategoriaContaMensalId = categoriaContaMensal.Id,
                    EmpresaId = empresa.Id
                };

                _context.Add(categoriaContaMensalEmpresa);
                _context.SaveChanges();

                _logger.LogInformation( "Criado registro de bloco na tabela terciario: "+
                                        "{CategoriaContaMensal}", categoriaContaMensal.Descricao);
                
                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return false;
            }
        }

        // GET: CategoriaContaMensal/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaContaMensal = await _context.CategoriaContaMensalEmpresa
                                                            .Where(c => c.EmpresaId == EmpresaUsuario.Id
                                                                && c.CategoriaContaMensalId == id 
                                                            )
                                                            .Include(c => c.CategoriaContaMensal)
                                                            .Select(c => c.CategoriaContaMensal)
                                                            .FirstOrDefaultAsync();

                _logger.LogInformation( "Abrindo edicao de CategoriaContaMensal: "+
                                        "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                if (categoriaContaMensal == null)
                {
                    return NotFound();
                }
                
                return View(categoriaContaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return RedirectToAction(nameof(Index));
            }
        }

        // POST: CategoriaContaMensal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, 
                                        [Bind("Id,Descricao")] CategoriaContaMensal categoriaContaMensal)
        {
            try{
                if (id != categoriaContaMensal.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(categoriaContaMensal);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CategoriaContaMensalExists(categoriaContaMensal.Id))
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
                return View(categoriaContaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return RedirectToAction(nameof(Index));
            }
        }

        // GET: CategoriaContaMensal/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var categoriaContaMensal = await _context.CategoriaContaMensalEmpresa
                                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id 
                                                                && c.CategoriaContaMensalId == id)
                                                    .Include(c => c.CategoriaContaMensal)
                                                    .Select(c => c.CategoriaContaMensal)
                                                    .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete da CategoriaContaMensal: "+
                "{CategoriaContaMensal}", categoriaContaMensal.Descricao);

                if (categoriaContaMensal == null)
                {
                    return NotFound();
                }

                return View(categoriaContaMensal);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return RedirectToAction(nameof(Index));
            }


        }

        // POST: CategoriaContaMensal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try{
                var categoriaContaMensal = await _context.CategoriaContaMensal.FindAsync(id);
                
                if(this.DeletaTerciaria(categoriaContaMensal, EmpresaUsuario))
                {
                    _context.CategoriaContaMensal.Remove(categoriaContaMensal);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeletaTerciaria(CategoriaContaMensal categoriaContaMensal, Empresa empresa)
        {
            try{
                CategoriaContaMensalEmpresa categoriaContaMensalEmpresa = _context
                                                                .CategoriaContaMensalEmpresa
                                                                .FirstOrDefault(c => 
                                                                c.CategoriaContaMensalId 
                                                                    == 
                                                                categoriaContaMensal.Id
                                                                && c.EmpresaId == empresa.Id)
                                                                ;

                _context.CategoriaContaMensalEmpresa.Remove(categoriaContaMensalEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return false;
            }
        }

        private bool CategoriaContaMensalExists(int id)
        {
            return _context.CategoriaContaMensal.Any(e => e.Id == id);
        }
    }
}
