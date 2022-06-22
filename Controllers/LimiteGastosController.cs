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
    public class LimiteGastosController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<LimiteGastosController> _logger;

        public LimiteGastosController(Contexto context, ILogger<LimiteGastosController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: LimiteGastos
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a página index");

                return View(await _context.LimiteGastosEmpresa
                                    .Where(l => l.EmpresaId == EmpresaUsuario.Id)
                                    .Include(l => l.LimiteGastos)
                                    .Select(l => l.LimiteGastos)
                                    .ToListAsync());
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LimiteGastos/Details/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var limiteGastos = await _context.LimiteGastosEmpresa
                                            .Where(l => l.EmpresaId  == EmpresaUsuario.Id
                                                    && l.LimiteGastosId == id)
                                            .Include(l => l.LimiteGastos)
                                            .Select(l => l.LimiteGastos)
                                            .FirstOrDefaultAsync(m => m.Id == id);

                if (limiteGastos == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes do LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                return View(limiteGastos);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LimiteGastos/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            try {
                List<String> ano = this.GetDropDownListForYears();

                ViewBag.Ano = ano;

                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: LimiteGastos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Limite")] LimiteGastos limiteGastos, List<String> Ano)
        {
            try {
                List<String> ano = this.GetDropDownListForYears();

                ViewBag.Ano = ano;
                
                DateTime DataAno = new DateTime(int.Parse(Ano[0]), 1, 1);

                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Criando registro do LimiteGastos: {LimiteGastos}",limiteGastos.Id);

                    limiteGastos.Ano = DataAno.Date;
                    limiteGastos.DataCriacao = DateTime.Now;

                    _context.Add(limiteGastos);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Criado registro do LimiteGastos: {LimiteGastos}",limiteGastos.Id);

                    if(this.CreateTerciaria(limiteGastos, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(limiteGastos);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(LimiteGastos limiteGastos, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro do LimiteGastos na tabela terciaria: {LimiteGastos}",limiteGastos.Id);

                LimiteGastosEmpresa limiteGastosEmpresa = new LimiteGastosEmpresa();

                int id = _context.LimiteGastosEmpresa.Select(l => l.Id).ToList().LastOrDefault();

                limiteGastosEmpresa.Id = id + 1;
                limiteGastosEmpresa.LimiteGastosId = limiteGastos.Id;
                limiteGastosEmpresa.EmpresaId = empresa.Id;

                _context.Add(limiteGastosEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro do LimiteGastos na tabela terciaria: {LimiteGastos}",limiteGastos.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: LimiteGastos/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                List<String> ano = this.GetDropDownListForYears();

                ViewBag.Ano = ano;

                var limiteGastos = await _context.LimiteGastosEmpresa
                                                .Where(l => l.EmpresaId == EmpresaUsuario.Id
                                                        && l.LimiteGastosId == id)
                                                .Include(l => l.LimiteGastos)
                                                .Select(l => l.LimiteGastos)
                                                .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo edicao de LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                if (limiteGastos == null)
                {
                    return NotFound();
                }

                return View(limiteGastos);                
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LimiteGastos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Limite,Ano,DataCriacao")] LimiteGastos limiteGastos)
        {
            try {
                if (id != limiteGastos.Id)
                {
                    return NotFound();
                }
                
                if (ModelState.IsValid)
                {
                    try
                    {
                        _logger.LogInformation("Atualizando LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                        List<String> ano = this.GetDropDownListForYears();

                        ViewBag.Ano = ano;

                        _context.Update(limiteGastos);
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Atualizando LimiteGastos: {LimiteGastos}", limiteGastos.Id);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LimiteGastosExists(limiteGastos.Id))
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
                return View(limiteGastos);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: LimiteGastos/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var limiteGastos = await _context.LimiteGastosEmpresa
                                            .Where(l => l.EmpresaId == EmpresaUsuario.Id
                                                    && l.LimiteGastosId == id)
                                            .Include(l => l.LimiteGastos)
                                            .Select(l => l.LimiteGastos)
                                            .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete do LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                if (limiteGastos == null)
                {
                    return NotFound();
                }

                return View(limiteGastos);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: LimiteGastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var limiteGastos = await _context.LimiteGastos.FindAsync(id);

                if(this.DeleteTerciaria(limiteGastos, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                    _context.LimiteGastos.Remove(limiteGastos);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado LimiteGastos: {LimiteGastos}", limiteGastos.Id);

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(LimiteGastos limiteGastos, Empresa empresa)
        {
            _logger.LogInformation("Deletando registro da tabela terciaria: {LimiteGastos}", limiteGastos.Id);

            try{
                LimiteGastosEmpresa limiteGastosEmpresa = _context
                                        .LimiteGastosEmpresa
                                        .FirstOrDefault(l => l.LimiteGastosId == limiteGastos.Id 
                                                        && l.EmpresaId == empresa.Id);

                _context.LimiteGastosEmpresa.Remove(limiteGastosEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;         
        }

        [Authorize(Roles = "Gestor")]
        private bool LimiteGastosExists(int id)
        {
            return _context.LimiteGastos.Any(e => e.Id == id);
        }

        [Authorize(Roles = "Gestor")]
        public LimiteGastos GetLimiteGastos()
        {
            try
            {
                return _context.LimiteGastos.FirstOrDefault(l => l.Ano.Year == DateTime.Now.Year);
            }
            catch
            {
                LimiteGastos limiteGastos = new LimiteGastos();

                limiteGastos.Limite = '0';

                return limiteGastos;
            }
        }

        [Authorize(Roles = "Gestor")]
        public List<String> GetDropDownListForYears()
        {
            List<String> ls = new List<String>();

            for (int i = 2021; i <= 2099; i++)
            {
                ls.Add(i.ToString());
            }

            return ls;
        }
    }
}
