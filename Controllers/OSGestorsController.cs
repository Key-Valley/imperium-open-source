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
    public class OSGestorsController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<OSGestorsController> _logger;

        public OSGestorsController(Contexto context, ILogger<OSGestorsController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: OSGestors
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a pagina Index");

                var contexto = _context.OSGestorEmpresa
                                    .Where(o => o.EmpresaId == EmpresaUsuario.Id)
                                    .Include(o => o.OSGestor)
                                    .Include(o => o.OSGestor.Equipamento)
                                    .Include(o => o.OSGestor.Fornecedor)
                                    .Include(o => o.OSGestor.Problematica)
                                    .Include(o => o.OSGestor.Status)
                                    .Select(o => o.OSGestor);

                int[] quant = new int[5];
                string[] label = new string[5] { "Em Espera", "Aprovada", "Negada", "Em Andamento", "Finalizada" };

                for (int i = 0; i < 5; i++)
                {
                    quant[i] = (from _OSGestor in _context.Set<OSGestor>()

                                where _OSGestor.StatusId == (i + 1)

                                select new { _OSGestor.StatusId }).Count();
                }

                ViewData["QtdOSStatus"] = quant;
                ViewData["QtdOSStatusLabel"] = label;

                return View(await contexto.ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: OSGestors/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var oSGestor = await _context.OSGestorEmpresa
                                        .Where(o => o.EmpresaId == EmpresaUsuario.Id && o.OSGestorId == id)
                                        .Include(o => o.OSGestor)
                                        .Include(o => o.OSGestor.Equipamento)
                                        .Include(o => o.OSGestor.Fornecedor)
                                        .Include(o => o.OSGestor.Problematica)
                                        .Include(o => o.OSGestor.Status)
                                        .Select(o => o.OSGestor)
                                        .FirstOrDefaultAsync();

                if (oSGestor == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes da OSGesto: ", oSGestor.Id);

                return View(oSGestor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: OSGestors/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create(int? id)
        {
            try {
                ViewData["EquipamentoId"] = new SelectList(_context.Equipamento, "Id", "Id");
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "Nome");

                var problems = _context.Problematica
                    .FromSqlInterpolated($"Select p.pro_id, p.pro_descricao, p.cat_id from tab_problematica p inner join tab_categoria c on p.cat_id = p.cat_id inner join tab_equipamento e on e.cat_id = c.cat_id and e.equ_id = {id}")
                    .ToList();

                ViewData["ProblematicaId"] = new SelectList(problems, "Id", "Descricao");
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao");

                _logger.LogInformation("Abrindo pagina de criacao de OSGestor");

                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: OSGestors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create(int? id, [Bind("Id,DataSolicitacao,DataAprovacao,DataConclusao,EquipamentoId,ProblematicaId,StatusId,FornecedorId,Valor")] OSGestor oSGestor)
        {
            try {
                DateTime hoje = DateTime.Now;
                oSGestor.EquipamentoId = (int)id;
                oSGestor.DataSolicitacao = hoje;
                oSGestor.DataAprovacao = hoje;
                oSGestor.DataConclusao = null;
                oSGestor.StatusId = 2;
                oSGestor.Valor = 0.0;

                if (ModelState.IsValid)
                {
                    if (this.EquipamentoTemOs(oSGestor.EquipamentoId))
                    {
                        //TODO: Criar página com o erro em tela para o usuario
                        return RedirectToAction(nameof(Create));
                    }
                    else
                    {
                        _logger.LogInformation("Criado registro OSGestor: {OSGestor}", oSGestor.Id);

                        _context.Add(oSGestor);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Criado registro OSGestor: {OSGestor}", oSGestor.Id);

                        if(this.CreateTerciaria(oSGestor, EmpresaUsuario))
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                ViewData["EquipamentoId"] = new SelectList(_context.Equipamento, "Id", "Id", oSGestor.EquipamentoId);
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "CNPJ", oSGestor.FornecedorId);
                ViewData["ProblematicaId"] = new SelectList(_context.Problematica, "Id", "Descricao", oSGestor.ProblematicaId);
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Descricao", oSGestor.StatusId);
                return View(oSGestor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(OSGestor osgestor, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro da OSGestor na tabela terciaria: {OSGestor}", osgestor.Id);

                OSGestorEmpresa OsGestorEmpresa = new OSGestorEmpresa();

                int id = _context.OSGestorEmpresa.Select(o => o.Id).ToList().LastOrDefault();

                OsGestorEmpresa.Id = id + 1;
                OsGestorEmpresa.OSGestorId = osgestor.Id;
                OsGestorEmpresa.EmpresaId = empresa.Id;

                _context.Add(OsGestorEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro da OSGestor na tabela terciaria: {OSGestor}", osgestor.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: OSGestors/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var oSGestor = await _context.OSGestorEmpresa
                                        .Where(o => o.EmpresaId == EmpresaUsuario.Id && o.OSGestorId == id)
                                        .Include(o => o.OSGestor)
                                        .Include(o => o.OSGestor.Equipamento)
                                        .Include(o => o.OSGestor.Fornecedor)
                                        .Include(o => o.OSGestor.Problematica)
                                        .Include(o => o.OSGestor.Status)
                                        .Select(o => o.OSGestor)
                                        .FirstOrDefaultAsync();

                if (oSGestor == null)
                {
                    return NotFound();
                }
                ViewData["EquipamentoId"] = new SelectList(_context.Equipamento, "Id", "Descricao", oSGestor.EquipamentoId);
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "CNPJ", oSGestor.FornecedorId);
                ViewData["ProblematicaId"] = new SelectList(_context.Problematica, "Id", "Descricao", oSGestor.ProblematicaId);
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Id", oSGestor.StatusId);

                _logger.LogInformation("Abrindo edicao de OSGestor: {OSGestor}", oSGestor.Id);

                return View(oSGestor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: OSGestors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DataSolicitacao,DataAprovacao,DataConclusao,EquipamentoId,ProblematicaId,StatusId,FornecedorId,Valor")] OSGestor oSGestor)
        {
            try {
                if (id != oSGestor.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(oSGestor);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OSGestorExists(oSGestor.Id))
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
                ViewData["EquipamentoId"] = new SelectList(_context.Equipamento, "Id", "Descricao", oSGestor.EquipamentoId);
                ViewData["FornecedorId"] = new SelectList(_context.Fornecedor, "Id", "CNPJ", oSGestor.FornecedorId);
                ViewData["ProblematicaId"] = new SelectList(_context.Problematica, "Id", "Descricao", oSGestor.ProblematicaId);
                ViewData["StatusId"] = new SelectList(_context.Status, "Id", "Id", oSGestor.StatusId);
                return View(oSGestor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: OSGestors/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var oSGestor = await _context.OSGestorEmpresa
                                        .Where(o => o.EmpresaId == EmpresaUsuario.Id && o.OSGestorId == id)
                                        .Include(o => o.OSGestor)
                                        .Include(o => o.OSGestor.Equipamento)
                                        .Include(o => o.OSGestor.Fornecedor)
                                        .Include(o => o.OSGestor.Problematica)
                                        .Include(o => o.OSGestor.Status)
                                        .Select(o => o.OSGestor)
                                        .FirstOrDefaultAsync();

                if (oSGestor == null)
                {
                    return NotFound();
                }

                return View(oSGestor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: OSGestors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var oSGestor = await _context.OSGestor.FindAsync(id);

                if(this.DeletaTerciaria(oSGestor, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando OSGestor: {OSGestor}", oSGestor.Id);

                    _context.OSGestor.Remove(oSGestor);
                    await _context.SaveChangesAsync();

                    _logger.LogError("Deletado OSGestor: {OSGestor}", oSGestor.Id);

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            } 

            return RedirectToAction(nameof(Index));
        }

        public bool DeletaTerciaria(OSGestor osgestor, Empresa empresa) 
        {
            _logger.LogInformation("Deletando registro da tabela terciaria: {OSGestor}",osgestor.Id);

            try{
                OSGestorEmpresa osgestorEmpresa = _context
                                                    .OSGestorEmpresa
                                                    .FirstOrDefault(o => o.OSGestorId == osgestor.Id 
                                                                    && o.EmpresaId == empresa.Id);

                _context.OSGestorEmpresa.Remove(osgestorEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool OSGestorExists(int id)
        {
            try {
                return _context.OSGestor.Any(e => e.Id == id);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return false;
            }
        }

        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> ChangeStatusOs(int? id)
        {
            try {
                var Os = await _context.OSGestorEmpresa
                                            .Where(o => o.EmpresaId == EmpresaUsuario.Id && o.OSGestorId == id)
                                            .Include(o => o.OSGestor)
                                            .Include(o => o.OSGestor.Equipamento)
                                            .Include(o => o.OSGestor.Fornecedor)
                                            .Include(o => o.OSGestor.Problematica)
                                            .Include(o => o.OSGestor.Status)
                                            .Select(o => o.OSGestor)
                                            .FirstOrDefaultAsync();

                if (id == null || Os == null)
                {
                    return NotFound();
                }

                if (Os.DataConclusao.Equals(null))
                {
                    Os.DataConclusao = DateTime.Today;
                    Os.StatusId = 5;
                    _context.Update(Os);
                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation("Atualizado com sucesso");

                return RedirectToAction("Index");
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public bool EquipamentoTemOs(int idEquipamento)
        {
            try
            {
                _logger.LogInformation("Verificando se o equipamento tem OS");

                var existeOs = _context.OSGestorEmpresa
                                .Where(o => o.EmpresaId == EmpresaUsuario.Id && o.OSGestor.EquipamentoId == idEquipamento)
                                .Include(o => o.OSGestor)
                                .Include(o => o.OSGestor.Equipamento)
                                .Include(o => o.OSGestor.Fornecedor)
                                .Include(o => o.OSGestor.Problematica)
                                .Include(o => o.OSGestor.Status)
                                .Select(o => o.OSGestor)
                                .FirstOrDefault();

                _logger.LogInformation("{ExisteOS}",existeOs);

                if (existeOs != null) 
                {
                    if (!existeOs.Equals(null) || !existeOs.Id.Equals(""))
                    {
                        return true;
                    }
                }

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            return false;
        }
    }
}
