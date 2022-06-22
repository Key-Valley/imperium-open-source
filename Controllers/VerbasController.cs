using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Controllers
{
    public class VerbasController : Controller
    {
        private readonly Contexto _context;

        public VerbasController(Contexto context)
        {
            _context = context;
        }

        // GET: Verbas
        public async Task<IActionResult> Index()
        {
            ViewBag.VerbaGasta = this.CalculaGastoMensais();

            return View(await _context.Verbas.ToListAsync());     
        }

        // GET: Verbas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Verbas = await _context.Verbas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Verbas == null)
            {
                return NotFound();
            }

            return View(Verbas);
        }

        // GET: Verbas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Verbas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Valor,Periodo")] Verbas Verbas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Verbas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Verbas);
        }

        // GET: Verbas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Verbas = await _context.Verbas.FindAsync(id);
            if (Verbas == null)
            {
                return NotFound();
            }
            return View(Verbas);
        }

        // POST: Verbas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Valor,Periodo")] Verbas Verbas)
        {
            if (id != Verbas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Verbas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VerbasExists(Verbas.Id))
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
            return View(Verbas);
        }

        // GET: Verbas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Verbas = await _context.Verbas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Verbas == null)
            {
                return NotFound();
            }

            return View(Verbas);
        }

        // POST: Verbas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Verbas = await _context.Verbas.FindAsync(id);
            _context.Verbas.Remove(Verbas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VerbasExists(int id)
        {
            return _context.Verbas.Any(e => e.Id == id);
        }

        public double GetGastos()
        {               
            Double soma = 0;
            
            List<OSGestor> lista_os = _context.OSGestor
                                            .Where(o => o.StatusId == 5)
                                            .ToList();

            foreach(var item in lista_os)
            {
                var DataConclusao = (item.DataConclusao).ToString();

                DataConclusao = DataConclusao.Substring(3,2);

                DataConclusao = DataConclusao.TrimStart('0');

                if(DateTime.Now.Month == int.Parse(DataConclusao))
                {
                    soma += soma + item.Valor;
                } 
            }

            return soma;
        }

        public double CalculaGastoMensais()
        {
            double somaDosGasto =  this.GetGastos();

            Verbas Verbas = _context.Verbas.Where(o => o.Periodo.Month == DateTime.Now.Month).FirstOrDefault();

            double VerbasDisponivel = Verbas.Valor - somaDosGasto;

            return VerbasDisponivel;
        }

  }
}
