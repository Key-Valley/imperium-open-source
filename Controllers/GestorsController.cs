using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Fatec_Facilities.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;

namespace Fatec_Facilities.Controllers
{
    public class GestorsController : Controller
    {
        private readonly Contexto _context;

        public GestorsController(Contexto context)
        {
            _context = context;
        }

        [Authorize(Roles = "Gestor,Usuario")]
        public IActionResult Home()
        {
            return View();
        }

        // GET: Gestors
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Index()
        {
            var cookieEmail = Request.Cookies["Email"];
            ViewData["Email"] = cookieEmail;

            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Gestors/Details/5
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestor = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gestor == null)
            {
                return NotFound();
            }

            return View(gestor);
        }

        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> EditProfile()
        {
            var cookieEmail = Request.Cookies["Email"];
            ViewData["Email"] = cookieEmail;

            Usuario usuario = await _context.Usuarios.FirstOrDefaultAsync(ges => ges.Email == cookieEmail);
            int id = usuario.Id;

            return RedirectToAction("Edit", new { id });
        }

        [Authorize]
        public IActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor,Usuario")]
        public async Task<IActionResult> EditPassword(string NovaSenha, [Bind("Senha")] Usuario usuario)
        {
            var cookieEmail = Request.Cookies["Email"];
            ViewData["Email"] = cookieEmail;
            var gestor1 = await _context.Usuarios.FirstOrDefaultAsync(ges => ges.Email == cookieEmail);

            Console.WriteLine(gestor1.Id);
            Console.WriteLine("Senha antiga: " + usuario.Senha);
            Console.WriteLine("Senha Nova: " + NovaSenha);
            if (gestor1 == null)
            {
                return NotFound();
            }

            if ((BC.Verify(usuario.Senha, gestor1.Senha)))
            {
                gestor1.Senha = BC.HashPassword(NovaSenha);
                _context.Update(gestor1);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Gestor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gestors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Senha,Rm")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Senha = BC.HashPassword(usuario.Senha);
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Gestors/Edit/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestor = await _context.Usuarios.FindAsync(id);
            if (gestor == null)
            {
                return NotFound();
            }
            return View(gestor);
        }

        // POST: Gestors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Senha,Email,Rm,Gestor")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            // if (ModelState.IsValid)
            // {
            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GestorExists(usuario.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            // }
            // return View(gestor);
        }

        // GET: Gestors/Delete/5
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gestor = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gestor == null)
            {
                return NotFound();
            }

            return View(gestor);
        }

        // POST: Gestors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gestor = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(gestor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Gestor,Usuario")]
        private bool GestorExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
