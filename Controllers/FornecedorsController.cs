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
    public class FornecedorsController : Controller
    {
        private readonly Contexto _context;
        private readonly LimiteGastosController limiteGastos;
        private readonly EmpresaController _empresaController;

        private Empresa EmpresaUsuario;

        private readonly ILogger<LimiteGastosController> _logger;

        public FornecedorsController(Contexto context, ILogger<LimiteGastosController> logger)
        {
            _context = context;
            _logger = logger;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;

            limiteGastos = new LimiteGastosController(_context, _logger);
        }

        // GET: Fornecedors
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Abrindo a pagina de index");

                List<Fornecedor> contexto = await _context.FornecedorEmpresa
                                                    .Where(f => f.EmpresaId == EmpresaUsuario.Id)
                                                    .Include(f => f.Fornecedor)
                                                    .Select(f => f.Fornecedor)
                                                    .ToListAsync();

                List<List<String>> fornecedoresCategoria = this.BuscaCategoriasFornecedor(contexto);

                ViewBag.FornecedoresCategoria = fornecedoresCategoria;

                var valorGasto = this.LimiteGasto();

                ViewBag.GastosFornecedores = valorGasto;

                ViewBag.LimiteGastos = limiteGastos.GetLimiteGastos();

                return View(contexto);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Fornecedors/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var fornecedor = await _context.FornecedorEmpresa
                                            .Where(f => f.FornecedorId == id && f.EmpresaId == EmpresaUsuario.Id)
                                            .Include(f => f.Fornecedor)
                                            .Select(f => f.Fornecedor)
                                            .ToListAsync();

                _logger.LogInformation("Abrindo a pagina de detalhes do Forncedor: {Fornecedor}", fornecedor[0].Id);

                List<List<String>> fornecedoresCategoria = this.BuscaCategoriasFornecedor(fornecedor);

                ViewBag.FornecedoresCategoria = fornecedoresCategoria[0];

                if (fornecedor == null)
                {
                    return NotFound();
                }

                return View(fornecedor.FirstOrDefault(f => f.Id == id));
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        // GET: Fornecedors/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        { 
            try {
                ViewBag.Categorias = _context.Categoria.ToList();

                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));     
        }

        // POST: Fornecedors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,CNPJ,Nome,Endereco,Contato,Prestador")] Fornecedor fornecedor, List<int> Categoria)
        {
            try {
                ViewBag.Categorias = _context.Categoria.ToList();

                if (ModelState.IsValid)
                {
                    
                    _context.Add(fornecedor);
                    await _context.SaveChangesAsync();
                    

                    if(this.CreateTerciaria(fornecedor, EmpresaUsuario))
                    {
                        bool inseridoCategorias = this.SalvarCategoriasFornecedor(_context.Fornecedor.ToList().Last().Id, Categoria);

                        return RedirectToAction(nameof(Index));
                    }
                }

                return View(fornecedor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));

        }

        public bool CreateTerciaria(Fornecedor fornecedor, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro na tabela terciaria para o Fornecedor: {Fornecedor}", fornecedor.Id);

                FornecedorEmpresa fornecedorEmpresa = new FornecedorEmpresa();

                int id = _context.FornecedorEmpresa.Select(f => f.Id).ToList().LastOrDefault();

                fornecedorEmpresa.Id = id + 1;
                fornecedorEmpresa.FornecedorId = fornecedor.Id;
                fornecedorEmpresa.EmpresaId = empresa.Id;

                _context.Add(fornecedorEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado o registro na tabela terciaria para o Fornecedor: {Fornecedor}", fornecedor.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: Fornecedors/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var fornecedor = await _context.FornecedorEmpresa
                                            .Where(f => f.EmpresaId == EmpresaUsuario.Id
                                                    && f.FornecedorId == id)
                                            .Include(f => f.Fornecedor)
                                            .Select(f => f.Fornecedor)
                                            .FirstOrDefaultAsync();
                if (fornecedor == null)
                {
                    return NotFound();
                }

                ViewBag.Categorias = _context.Categoria.ToList();
                
                return View(fornecedor);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Fornecedors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CNPJ,Nome,Endereco,Contato,CategoriaId,Prestador")] Fornecedor fornecedor, List<int> Categoria)
        {
            ViewBag.Categorias = _context.Categoria.ToList();

            if (id != fornecedor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this.ExcluirFornecedorCategorias(fornecedor);
                    this.SalvarCategoriasFornecedor(fornecedor.Id, Categoria);
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.Id))
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
            
            return View(fornecedor);
        }

        // GET: Fornecedors/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var fornecedor = await _context.FornecedorEmpresa
                                    .Where(f => f.EmpresaId == EmpresaUsuario.Id
                                            && f.FornecedorId == id)
                                    .Include(f => f.Fornecedor)
                                    .Select(f => f.Fornecedor)
                                    .FirstOrDefaultAsync();

                var fornecedorDeletado = _context.FornecedorEmpresa
                                            .Where(f => f.EmpresaId == EmpresaUsuario.Id
                                                    && f.FornecedorId == id)
                                            .Include(f => f.Fornecedor)
                                            .Select(f => f.Fornecedor)
                                            .ToList();

                List<List<String>> fornecedoresCategoria = this.BuscaCategoriasFornecedor(fornecedorDeletado);

                ViewBag.FornecedoresCategoria = fornecedoresCategoria[0];

                if (fornecedor == null)
                {
                    return NotFound();
                }

                return View(fornecedor);
            } catch(Exception e) {
                _logger.LogInformation(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Fornecedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try {
                var fornecedor = await _context.FornecedorEmpresa
                                        .Where(f => f.EmpresaId == EmpresaUsuario.Id
                                                && f.FornecedorId == id)
                                        .Include(f => f.Fornecedor)
                                        .Select(f => f.Fornecedor)
                                        .FirstOrDefaultAsync();

                var fornecedorDeletado = _context.FornecedorEmpresa
                                .Where(f => f.EmpresaId == EmpresaUsuario.Id
                                        && f.FornecedorId == id)
                                .Include(f => f.Fornecedor)
                                .Select(f => f.Fornecedor)
                                .ToList();

                List<List<String>> fornecedoresCategoria = this.BuscaCategoriasFornecedor(fornecedorDeletado);

                ViewBag.FornecedoresCategoria = fornecedoresCategoria[0];

                bool result = this.ExcluirFornecedorCategorias(fornecedor);

                if(!result)
                {
                    return RedirectToAction(nameof(Index));
                }

                if(this.DeleteTerciaria(fornecedor, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando Fornecedor: {Fornecedor}", fornecedor.Id);

                    _context.Fornecedor.Remove(fornecedor);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado Fornecedor: {Fornecedor}", fornecedor.Id);
                }

                return RedirectToAction(nameof(Index));
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(Fornecedor fornecedor, Empresa empresa)
        {

            _logger.LogInformation("Deletando registro da tabela terciaria: {Forneceodr}", fornecedor.Id);

            try {
                FornecedorEmpresa fornecedorEmpresa = _context.FornecedorEmpresa
                                                        .FirstOrDefault(l => l.FornecedorId == fornecedor.Id
                                                                        && l.EmpresaId == EmpresaUsuario.Id);

                _context.FornecedorEmpresa.Remove(fornecedorEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool FornecedorExists(int id)
        {
            try {
                return _context.Fornecedor.Any(e => e.Id == id);
            } catch(Exception e) {
                _logger.LogError(e.ToString());

                return false;
            }
        }

        private List<List<string>> BuscaCategoriasFornecedor(List<Fornecedor> Fornecedores)
        { 
            try {
                List<List<String>> todosFornecedores = new List<List<String>>();

                if (Fornecedores != null)
                {
                    foreach (Fornecedor fornecedor in Fornecedores)
                    {
                        List<String> todasCategorias = new List<String>();

                        var catFor = _context.Categoria
                                                    .Join(_context.FornecedorCategoria, categoria => categoria.Id, fornCat => fornCat.CategoriaId, (categoria, fornCat) => new { categoria, fornCat })
                                                    .Join(_context.Fornecedor, fornCat => fornCat.fornCat.FornecedorId, fornecedor => fornecedor.Id, (fornCat, fornecedor) => new { fornCat, fornecedor })
                                                    .Where(f => f.fornecedor.Id == fornecedor.Id)
                                                    .Select(x => new { x.fornCat.fornCat.Categoria });

                        var categoriaFornecedor = catFor.ToList();

                        todasCategorias.Add(fornecedor.Id.ToString());

                        foreach (var cat in categoriaFornecedor)
                        {
                            todasCategorias.Add(cat.Categoria.Nome);
                        }

                        todosFornecedores.Add(todasCategorias);
                    }
                }

                return todosFornecedores;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return null;
        }

        private bool SalvarCategoriasFornecedor(int id, List<int> categorias)
        {
            try
            {
                for(int i = 0; i < categorias.Count; i++)
                {
                    FornecedorCategoria fornecedorCategoria = new FornecedorCategoria();

                    fornecedorCategoria.FornecedorId = id;
                    fornecedorCategoria.CategoriaId = categorias[i];

                    _context.FornecedorCategoria.Add(fornecedorCategoria);

                    _context.SaveChanges();
                }

                return true;
            } catch(Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        private bool ExcluirFornecedorCategorias(Fornecedor fornecedor)
        {
            try
            {
                List<FornecedorCategoria> fornecedorCategoria = _context.FornecedorCategoria
                                                .Where(f => f.FornecedorId == fornecedor.Id)
                                                .ToList();

                foreach(var f in fornecedorCategoria )
                {
                    _context.FornecedorCategoria.Remove(f);
                    _context.SaveChanges();
                }

                return true;
            } catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        public Array LimiteGasto()
        {
            try {
                var gastosFornecedor = (
                    from _OSGestor in _context.Set<OSGestor>()
                    join _Fornecedor in _context.Set<Fornecedor>() on _OSGestor.FornecedorId equals _Fornecedor.Id
                    group _OSGestor by new { _Fornecedor.Id } into gasto
                    select new { gasto.Key.Id, gasto = gasto.Sum(p => p.Valor) }
                ).ToArray();

                return gastosFornecedor;
            } catch(Exception e) {
                _logger.LogInformation(e.ToString());
            }

            return null;
        }

    }

}
