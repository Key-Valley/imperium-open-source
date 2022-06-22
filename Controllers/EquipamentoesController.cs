using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Fatec_Facilities.Controllers
{
    public class EquipamentoesController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<EquipamentoesController> _logger;

        public EquipamentoesController(Contexto context, ILogger<EquipamentoesController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Equipamentos(int? id, string nomeCategoria, bool? redo)
        {
            try {
                ViewBag.Categorias = (from c in _context.Categoria select c.Nome).Distinct();

                if (nomeCategoria == null || redo == true)
                {
                    var equipamentos = _context.EquipamentoEmpresa
                                            .Where(e => e.Equipamento.LocalId == id 
                                                    && e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Select(e => e.Equipamento)
                                            .ToArrayAsync();

                    _logger.LogInformation("Carregando a pagina index de Equipamentos");

                    return View(await equipamentos);
                }
                else
                {
                    var IdCategoria = _context.CategoriaEmpresa
                                            .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Categoria)
                                            .Select(e => e.Categoria)
                                            .FirstOrDefault(c => c.Nome == nomeCategoria);
                    
                    var equipamentosf = _context.EquipamentoEmpresa
                                            .Where( b => b.Equipamento.LocalId == id 
                                                    && b.Equipamento.CategoriaId == IdCategoria.Id
                                                    && b.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Select(e => e.Equipamento)
                                            .ToArrayAsync();

                    _logger.LogInformation( "Carregando a pagina index de Equipamentos: {Equipamento} "+
                                            "e da Categoria: {Categoria}", 
                                            equipamentosf.Id,
                                            IdCategoria.Id);

                    return View(await equipamentosf);
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Equipamentoes
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {
                var contexto = _context.EquipamentoEmpresa
                                    .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                    .Include(e => e.Equipamento)
                                    .Include(e => e.Equipamento.Bloco)
                                    .Include(e => e.Equipamento.Categoria)
                                    .Include(e => e.Equipamento.Local)
                                    .Select(e => e.Equipamento);
                
                _logger.LogInformation("Carregando a pagina index de Equipamentos");

                return View(await contexto.ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Equipamentoes/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var equipamento = await _context.EquipamentoEmpresa
                    .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                    .Include(e => e.Equipamento)
                    .Include(e => e.Equipamento.Bloco)
                    .Include(e => e.Equipamento.Categoria)
                    .Include(e => e.Equipamento.Local)
                    .Select(e => e.Equipamento)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (equipamento == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes do equipamento: {Equipamento}", equipamento.Id);

                return View(equipamento);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> DetalhesParaOS(int? id)
        {
            try{
                if (id == null)
                {
                    return NotFound();
                }

                var equipamento = await _context.EquipamentoEmpresa
                                            .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Include(e => e.Equipamento.Bloco)
                                            .Include(e => e.Equipamento.Categoria)
                                            .Include(e => e.Equipamento.Local)
                                            .Select(e => e.Equipamento)
                                            .FirstOrDefaultAsync(m => m.Id == id);

                _logger.LogInformation("Abrindo a pagina de detalhes para OS: {Equipamento}", equipamento.Id);

                if (equipamento == null)
                {
                    return NotFound();
                }

                return View(equipamento);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Equipamentoes/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            try {
                ViewData["BlocoId"] = new SelectList(
                                                _context.BlocoEmpresa
                                                .Where(b => b.EmpresaId == EmpresaUsuario.Id)
                                                .Include(b => b.Bloco)
                                                .Select(b => b.Bloco), 
                                                "Id", 
                                                "Nome");

                ViewData["CategoriaId"] = new SelectList(
                                                _context.CategoriaEmpresa
                                                .Where(c => c.EmpresaId == EmpresaUsuario.Id)
                                                .Include(c => c.Categoria)
                                                .Select(c => c.Categoria), 
                                                "Id", 
                                                "Nome");

                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Gestor,Usuario")]
        public JsonResult LoadLocal(int? id)
        {
            try {
                var locais = _context.Local
                                 .Where(b => b.BlocoID == id)
                                 .OrderBy(b => b.Nome)
                                 .ToList();
                return Json(locais);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return Json(nameof(Index));
        }

        // POST: Equipamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Registro,Descricao,Ativo,CategoriaId,BlocoId,LocalId")] Equipamento equipamento)
        {
            try {
                if (ModelState.IsValid)
                {
                    _context.Add(equipamento);
                    await _context.SaveChangesAsync();

                    if(this.CreateTerciaria(equipamento, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                ViewData["BlocoId"] = new SelectList(
                                                _context.BlocoEmpresa
                                                        /*.Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                                        .Include(e => e.Bloco)
                                                        .Select(e => e.Bloco)
                                                        .FirstOrDefault()*/,
                                                "Id",
                                                "Nome", 
                                                equipamento.BlocoId
                                                );
                ViewData["CategoriaId"] = new SelectList(
                                                _context.CategoriaEmpresa
                                                        /*.Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                                        .Include(e => e.Categoria)
                                                        .Select(e => e.Categoria)
                                                        .FirstOrDefault()*/, 
                                                "Id", 
                                                "Nome", 
                                                equipamento.CategoriaId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", equipamento.LocalId);

                return View(equipamento);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(Equipamento equipamento, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro do bloco na tabela terciria: {Equipamento}", equipamento.Id);

                EquipamentoEmpresa equipamentoEmpresa = new EquipamentoEmpresa();

                int id = _context.EquipamentoEmpresa.Select(b => b.Id).ToList().LastOrDefault();

                equipamentoEmpresa.Id = id + 1;
                equipamentoEmpresa.EquipamentoId = equipamento.Id;
                equipamentoEmpresa.EmpresaId = empresa.Id;

                _context.Add(equipamentoEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro na tabela terciaria {Equipamento}", equipamento.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            try {
                var equipamento = await _context.EquipamentoEmpresa
                                                .Where(e => e.EmpresaId == EmpresaUsuario.Id && e.EquipamentoId == id)
                                                .Include(e => e.Equipamento)
                                                .Select(e => e.Equipamento)
                                                .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de mostrar o status do equipamento: {Equipamento}",equipamento.Id);

                if (id == null || equipamento == null)
                {
                    return NotFound();
                }

                if (equipamento.Ativo == true)
                {
                    equipamento.Ativo = false;
                    _context.Update(equipamento);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    equipamento.Ativo = true;
                    _context.Update(equipamento);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Equipamentoes/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var equipamento = await _context.EquipamentoEmpresa
                                            .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Select(e => e.Equipamento)
                                            .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de edicao do equipamento: {Equipamento}", equipamento.Id);

                if (equipamento == null)
                {
                    return NotFound();
                }

                ViewData["BlocoId"] = new SelectList(_context.Blocos, "Id", "Nome", equipamento.BlocoId);
                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", equipamento.CategoriaId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", equipamento.LocalId);
                return View(equipamento);

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Equipamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Registro,Descricao,Ativo,CategoriaId,BlocoId,LocalId")] Equipamento equipamento)
        {
            _logger.LogInformation("Atualizando os registros do equipamento: {Equipamento}", equipamento.Id);

            try {
                if (id != equipamento.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(equipamento);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EquipamentoExists(equipamento.Id))
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
                ViewData["BlocoId"] = new SelectList(_context.Blocos, "Id", "Nome", equipamento.BlocoId);
                ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nome", equipamento.CategoriaId);
                ViewData["LocalId"] = new SelectList(_context.Local, "Id", "Nome", equipamento.LocalId);
                return View(equipamento);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Equipamentoes/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var equipamento = await _context.EquipamentoEmpresa
                                            .Where(e => e.EquipamentoId == id
                                                    && e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Include(e => e.Equipamento.Bloco)
                                            .Include(e => e.Equipamento.Categoria)
                                            .Include(e => e.Equipamento.Local)
                                            .Select(e => e.Equipamento)
                                            .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete do equipamento: {Equipamento}", equipamento.Id);

                if (equipamento == null)
                {
                    return NotFound();
                }

                return View(equipamento);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Equipamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var equipamento = await _context.EquipamentoEmpresa
                                            .Where(e => e.EquipamentoId == id
                                                    && e.EmpresaId == EmpresaUsuario.Id)
                                            .Include(e => e.Equipamento)
                                            .Include(e => e.Equipamento.Bloco)
                                            .Include(e => e.Equipamento.Categoria)
                                            .Include(e => e.Equipamento.Local)
                                            .Select(e => e.Equipamento)
                                            .FirstOrDefaultAsync();

                _logger.LogInformation("Deletando registro na tabela: {Equipamento}", equipamento.Id);

                if(this.DeleteTerciaria(equipamento, EmpresaUsuario)) {
                    _context.Equipamento.Remove(equipamento);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(Equipamento equipamento, Empresa empresa) 
        {
            _logger.LogInformation("Deletando registro na tabela terciaria: {Equipamento}", equipamento.Id);

            try {
                EquipamentoEmpresa equipamentoEmpresa = _context.EquipamentoEmpresa
                                                                .FirstOrDefault(e => e.EquipamentoId == equipamento.Id
                                                                                && e.EmpresaId == empresa.Id);

                _context.EquipamentoEmpresa.Remove(equipamentoEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool EquipamentoExists(int id)
        {
            try {
                return _context.Equipamento.Any(e => e.Id == id);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
                
                return false;
            }
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private void FiltrarEquipamentosCategoria(string idCategoria)
        {
            var cat = idCategoria;
        }
    }


}
