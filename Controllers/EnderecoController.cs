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
    public class EnderecoController : Controller
    {
        private readonly Contexto _context;
        EmpresaController _empresaController;

        private static Empresa EmpresaUsuario;

        private readonly ILogger<EnderecoController> _logger;

        public EnderecoController(Contexto context, ILogger<EnderecoController> logger)
        {
            _context = context;

            _empresaController = new EmpresaController(_context);

            EmpresaUsuario = _empresaController.getEmpresaUsuarioLogado();

            _logger = logger;
        }

        // GET: Endereco
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            try {

                _logger.LogInformation("Carregando a página index");

                return View(await _context.EnderecoEmpresa
                                .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                .Include(e => e.Endereco)
                                .Select(e => e.Endereco)
                                .ToListAsync());

            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Endereco/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var endereco = await _context.EnderecoEmpresa
                                        .Where(e => e.EmpresaId == EmpresaUsuario.Id)
                                        .Include(e => e.Endereco)
                                        .Select(e => e.Endereco)
                                        .FirstOrDefaultAsync(m => m.Id == id);

                if (endereco == null)
                {
                    return NotFound();
                }

                _logger.LogInformation("Abrindo a pagina de detalhes do Endereco: {Endereco}", endereco.Id);

                return View(endereco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Endereco/Create
        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            try {   
                _logger.LogInformation("Acessando a pagina de criacao de endereco");

                return View();
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Endereco/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Logradouro,Numero,Bairro,Cep,Complemento,Cidade,Estado,Pais")] Endereco endereco)
        {
            try {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation("Criando registro do endereço: {Endereco}", endereco.Id);

                    _context.Add(endereco);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Criado registro do endereço: {Endereço}", endereco.Id);

                    if(this.CreateTerciaria(endereco, EmpresaUsuario))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                return View(endereco);
           } catch(Exception e) {
               _logger.LogError(e.ToString());
           }

           return RedirectToAction(nameof(Index));
        }

        public bool CreateTerciaria(Endereco endereco, Empresa empresa)
        {
            try {
                _logger.LogInformation("Criando registro do endereco na tabela terciaria: {Endereco}", endereco.Id);

                EnderecoEmpresa enderecoEmpresa = new EnderecoEmpresa();

                int id = _context.EnderecoEmpresa.Select(e => e.Id).ToList().LastOrDefault();

                enderecoEmpresa.Id = id+1;
                enderecoEmpresa.EnderecoId = endereco.Id;
                enderecoEmpresa.EmpresaId = empresa.Id;

                _context.Add(enderecoEmpresa);
                _context.SaveChanges();

                _logger.LogInformation("Criado registro de endereco na tabela terciaria: {Endereco}", endereco.Id);

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        // GET: Endereco/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var endereco = await _context.Endereco.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            return View(endereco);
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logradouro,Numero,Bairro,Cep,Complemento,Cidade,Estado,Pais")] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endereco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecoExists(endereco.Id))
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
            return View(endereco);
        }

        // GET: Endereco/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try {
                if (id == null)
                {
                    return NotFound();
                }

                var endereco = await _context.EnderecoEmpresa
                                        .Where(e => e.EmpresaId == EmpresaUsuario.Id && e.EnderecoId == id)
                                        .Include(e => e.Endereco)
                                        .Select(e => e.Endereco)
                                        .FirstOrDefaultAsync();

                _logger.LogInformation("Abrindo a pagina de delete do endereco: {Endereco}", endereco.Id);

                if (endereco == null)
                {
                    return NotFound();
                }

                return View(endereco);
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var endereco = await _context.Endereco.FindAsync(id);

            try{
                if(this.DeleteTerciaria(endereco, EmpresaUsuario))
                {
                    _logger.LogInformation("Deletando endereco: {Endereco}", endereco.Id);

                    _context.Endereco.Remove(endereco);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Deletado o endereco: {Endereco}", endereco.Id);

                    return RedirectToAction(nameof(Index));
                }
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }   

            return RedirectToAction(nameof(Index));
        }

        public bool DeleteTerciaria(Endereco endereco, Empresa empresa)
        {
            _logger.LogInformation("Deletando registro da tabela terciaria: {Endereco}", endereco.Id);

            try {
                EnderecoEmpresa enderecoEmpresa = _context
                                        .EnderecoEmpresa
                                        .FirstOrDefault(e => e.EnderecoId == endereco.Id 
                                                        &&  e.EmpresaId == empresa.Id);

                _context.EnderecoEmpresa.Remove(enderecoEmpresa);

                _context.SaveChanges();

                return true;
            } catch(Exception e) {
                _logger.LogError(e.ToString());
            }

            return false;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool EnderecoExists(int id)
        {
            return _context.Endereco.Any(e => e.Id == id);
        }
    }
}
