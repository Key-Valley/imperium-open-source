using Fatec_Facilities.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fatec_Facilities.Controllers
{
    public class UsuarioEmpresaController : Controller
    {
        private readonly Contexto _context;

        public UsuarioEmpresaController(Contexto context)
        {
            _context = context;
        }

        public async Task Create(Usuario usuario, Empresa empresa)
        {
            UsuarioEmpresa usuarioEmpresa = new UsuarioEmpresa();

            UsuarioEmpresa usuarioEmpresaUltimoId = _context.UsuarioEmpresa.ToList().LastOrDefault();

            usuarioEmpresa.Id = (usuarioEmpresaUltimoId.Id) + 1;
            usuarioEmpresa.UsuarioId = usuario.Id;
            usuarioEmpresa.EmpresaId = empresa.Id;

            _context.Add(usuarioEmpresa);

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            UsuarioEmpresa usuarioEmpresa = _context.UsuarioEmpresa.FirstOrDefault(usuEmp => usuEmp.UsuarioId == id);

            _context.UsuarioEmpresa.Remove(usuarioEmpresa);

            await _context.SaveChangesAsync();
        }
    }
}
