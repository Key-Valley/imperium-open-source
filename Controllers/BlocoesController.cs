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
    public class BlocoesController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<BlocoesController> _logger;

        public BlocoesController(Contexto context, ILogger<BlocoesController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public JsonResult LoadEquipaments()
        {
            try {

                var equipaments = _context.Equipamento
                .GroupBy(e => e.BlocoId)
                .Select(g => new {id = g.Key, qtd = g.Count() }).ToList();

                _logger.LogInformation("Carregando os equipamentos...");

                return Json(equipaments);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return Json(nameof(Index));
        }

        //GET: Mapa
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Mapa()
        {
            try {
                _logger.LogInformation("Carregando o mapa da empresa: {EmpresaId}", EmpresaUsuario);

                return View(await _context.BlocoEmpresa
                                    .Where(b => b.EmpresaId == EmpresaUsuario.Id)
                                    .Include(e => e.Bloco)
                                    .Select(e => e.Bloco)
                                    .ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Blocoes
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try{

                _logger.LogInformation("Carregando a página index");

                return View(await _context.BlocoEmpresa
                                .Where(b => b.EmpresaId == EmpresaUsuario.Id)
                                .Include(e => e.Bloco)
                                .Select(e => e.Bloco)
                                .ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }                  

            return RedirectToAction(nameof(Index)); 
        }

        // GET: Blocoes/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var bloco = await _context.BlocoEmpresa
                                    .Where(b => b.EmpresaId == EmpresaUsuario.Id 
                                                && b.BlocoId == id)
                                    .Include(e => e.Bloco)
                                    .Select(e => e.Bloco)
                                    .FirstOrDefaultAsync(m => m.Id == id);
                if (bloco == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes do bloco: {Bloco}", bloco.Nome);

                return View(bloco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Blocoes/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            _logger.LogInformation("Acessando pagina de criacao de bloco");

            return View();
        }

        // POST: Blocoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Bloco bloco)
        {
            try{
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Criando registro do bloco: {Bloco}", bloco.Nome);

                    _context.Add(bloco);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Criado registro do bloco: {Bloco}", bloco.Nome);

                    if(this.CreateTerciaria(bloco, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(bloco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }    

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(Bloco bloco, Empresa empresa)
        {
            try {
                _logger.LogInformation( "Criando registro do bloco na tabela terciaria: "+
                                        "{Bloco}", bloco.Nome);

                BlocoEmpresa blocoEmpresa = new BlocoEmpresa();

                int id = _context.BlocoEmpresa.Select(b => b.Id).ToList().LastOrDefault();

                blocoEmpresa.Id = id + 1;
                blocoEmpresa.BlocoId = bloco.Id;
                blocoEmpresa.EmpresaId = empresa.Id;

                _context.Add(blocoEmpresa);
                _context.SaveChanges();

                _logger.LogInformation( "Criado registro do bloco na tabela terciaria: "+
                                        "{Bloco}", bloco.Nome);

                return true;
            } catch (Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: Blocoes/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var bloco = await _context.BlocoEmpresa
                                        .Where(b => b.EmpresaId == EmpresaUsuario.Id && b.BlocoId == id)
                                        .Include(e => e.Bloco)
                                        .Select(e => e.Bloco)
                                        .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo edicao de bloco: {Bloco}", bloco.Nome);

                if (bloco == null)
                {
                    return NotFound();
                }
                
                return View(bloco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Blocoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Bloco bloco)
        {
            try{
                if (id != bloco.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _logger.LogInformation("Atualizando bloco: {Bloco}", bloco.Nome);

                        _context.Update(bloco);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Atualizado bloco: {Bloco}", bloco.Nome);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BlocoExists(bloco.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return View(bloco);
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Blocoes/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var bloco = await _context.BlocoEmpresa
                                    .Where(c => c.EmpresaId == EmpresaUsuario.Id && c.BlocoId == id)
                                    .Include(c => c.Bloco)
                                    .Select(c => c.Bloco)
                                    .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete do bloco: {Bloco}", bloco.Nome);

                if (bloco == null)
                {
                    return NotFound();
                }

                return View(bloco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Blocoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try{

                var bloco = await _context.Blocos.FindAsync(id);
            
                if(this.DeleteTerciaria(bloco, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando bloco: {Bloco}", bloco.Nome);

                    _context.Blocos.Remove(bloco);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado o bloco: {Bloco}", bloco.Nome);

                    return View(bloco); 
                }
            } catch (Exception e){
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(Bloco bloco, Empresa empresa) 
        {
            _logger.LogInformation("Deletando registro da tabela terciaria: {BlocoEmpresa}",bloco.Nome);

            try{
                BlocoEmpresa blocoEmpresa = _context
                                    .BlocoEmpresa
                                    .FirstOrDefault(b => b.BlocoId == bloco.Id 
                                                    && b.EmpresaId == empresa.Id);

                _context.BlocoEmpresa.Remove(blocoEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool BlocoExists(int id)
        {
            try{
                
                return _context.Blocos.Any(e => e.Id == id);

            } catch(Exception e)
            {
                _logger.LogError(e.ToString());

                return false;
            }
        }
    }
}
