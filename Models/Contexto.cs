using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fatec_Facilities.Models;

namespace Fatec_Facilities.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
            InicializaBD.Initialize(this);
        }

        //seta gestores e supervisores no banco
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Bloco> Blocos { get; set; }
        public DbSet<Local> Local { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Equipamento> Equipamento { get; set; }
        public DbSet<Problematica> Problematica { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<OSGestor> OSGestor { get; set; }
        public DbSet<SaveToken> SaveToken { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<UsuarioEmpresa> UsuarioEmpresa { get; set; }
        public DbSet<CategoriaManutencaoPredial> CategoriaManutencaoPredial { get; set; }
        public DbSet<ManutencaoPredial> ManutencaoPredial { get; set; }
        public DbSet<ContaMensal> ContaMensal { get; set; }
        public DbSet<CategoriaContaMensal> CategoriaContaMensal { get; set; }
        public DbSet<FornecedorCategoria> FornecedorCategoria { get; set; }
        public DbSet<LimiteGastos> LimiteGastos { get; set;}    
        public DbSet<Verbas> Verbas { get; set; }
        public DbSet<BlocoEmpresa> BlocoEmpresa { get; set; }
        public DbSet<CategoriaEmpresa> CategoriaEmpresa { get; set; }
        public DbSet<CategoriaContaMensalEmpresa> CategoriaContaMensalEmpresa { get; set; }
        public DbSet<CategoriaManutencaoPredialEmpresa> CategoriaManutencaoPredialEmpresa { 
            get; set;
        }
        public DbSet<ContaMensalEmpresa> ContaMensalEmpresa { get; set; }
        public DbSet<EnderecoEmpresa> EnderecoEmpresa { get; set; }
        public DbSet<EquipamentoEmpresa> EquipamentoEmpresa { get; set; }
        public DbSet<LimiteGastosEmpresa> LimiteGastosEmpresa { get; set; }
        public DbSet<LocalEmpresa> LocalEmpresa { get; set; }
        public DbSet<FornecedorEmpresa> FornecedorEmpresa { get; set; }
        public DbSet<ManutencaoPredialEmpresa> ManutencaoPredialEmpresa { get; set; }
        public DbSet<OSGestorEmpresa> OSGestorEmpresa { get; set; }

        public DbSet<ProblematicaEmpresa> ProblematicaEmpresa { get; set; }
        
    }
}
