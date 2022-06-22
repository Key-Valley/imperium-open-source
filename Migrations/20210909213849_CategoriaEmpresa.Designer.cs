﻿// <auto-generated />
using System;
using Fatec_Facilities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fatec_Facilities.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20210909213849_CategoriaEmpresa")]
    partial class CategoriaEmpresa
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Fatec_Facilities.Models.Bloco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("blo_id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("blo_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_bloco");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.BlocoEmpresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("blo_emp_id")
                        .HasColumnType("int");

                    b.Property<int>("BlocoId")
                        .HasColumnName("blo_id")
                        .HasColumnType("int");

                    b.Property<int>("EmpresaId")
                        .HasColumnName("emp_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlocoId");

                    b.HasIndex("EmpresaId");

                    b.ToTable("tab_bloco_empresa");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("cat_id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("cat_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_categoria");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.CategoriaContaMensal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ccm_id")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnName("ccm_descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_categoria_conta_mensal");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.CategoriaEmpresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("cat_emp_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnName("cat_id")
                        .HasColumnType("int");

                    b.Property<int>("EmpresaId")
                        .HasColumnName("emp_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("EmpresaId");

                    b.ToTable("tab_categoria_empresa");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.CategoriaManutencaoPredial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("cmp_id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("cmp_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_categoria_manutencao_predial");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.ContaMensal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("con_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaContaMensalId")
                        .HasColumnName("ccm_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataPagamento")
                        .HasColumnName("con_date_pagamento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataVencimento")
                        .HasColumnName("con_data_vencimento")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Valor")
                        .HasColumnName("con_valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaContaMensalId");

                    b.ToTable("tab_conta_mensal");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("emp_id")
                        .HasColumnType("int");

                    b.Property<string>("Cnpj")
                        .HasColumnName("emp_cnpj")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("EnderecoId")
                        .HasColumnName("end_id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .HasColumnName("emp_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NomeFantasia")
                        .HasColumnName("emp_nome_fantasia")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("EnderecoId");

                    b.ToTable("tab_empresa");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("end_id")
                        .HasColumnType("int");

                    b.Property<string>("Bairro")
                        .HasColumnName("end_bairro")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Cep")
                        .HasColumnName("end_cep")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Cidade")
                        .HasColumnName("end_cidade")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Complemento")
                        .HasColumnName("end_complemento")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Estado")
                        .HasColumnName("end_estado")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Logradouro")
                        .HasColumnName("end_logradouro")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Numero")
                        .HasColumnName("end_numero")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Pais")
                        .HasColumnName("end_pais")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_endereco");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Equipamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("equ_id")
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnName("equ_ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("BlocoId")
                        .HasColumnName("blo_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnName("cat_id")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnName("equ_descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("LocalId")
                        .HasColumnName("loc_id")
                        .HasColumnType("int");

                    b.Property<string>("Registro")
                        .IsRequired()
                        .HasColumnName("equ_registro")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("BlocoId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("LocalId");

                    b.ToTable("tab_equipamento");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Fornecedor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("for_id")
                        .HasColumnType("int");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnName("for_cnpj")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Contato")
                        .IsRequired()
                        .HasColumnName("for_contato")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnName("for_endereco")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("for_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Prestador")
                        .HasColumnName("for_prestador")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("tab_fornecedor");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.FornecedorCategoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("for_cat_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnName("cat_id")
                        .HasColumnType("int");

                    b.Property<int>("FornecedorId")
                        .HasColumnName("for_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("FornecedorId");

                    b.ToTable("tab_fornecedor_categoria");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.LimiteGastos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("lim_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Ano")
                        .HasColumnName("lim_ano")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataCriacao")
                        .HasColumnName("lim_data_criacao")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Limite")
                        .HasColumnName("lim_limite_anual")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("tab_limite_gastos");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Local", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("loc_id")
                        .HasColumnType("int");

                    b.Property<int>("BlocoID")
                        .HasColumnName("blo_id")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("loc_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("BlocoID");

                    b.ToTable("tab_local");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.ManutencaoPredial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("man_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaManutencaoPredialId")
                        .HasColumnName("cmp_id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataAprovacao")
                        .HasColumnName("man_data_aprovacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataConclusao")
                        .HasColumnName("man_data_conclusao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataSolicitacao")
                        .HasColumnName("man_data_solicitacao")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Descricao")
                        .HasColumnName("man_descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("FornecedorId")
                        .HasColumnName("for_id")
                        .HasColumnType("int");

                    b.Property<int>("LocalId")
                        .HasColumnName("loc_id")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnName("sta_id")
                        .HasColumnType("int");

                    b.Property<double>("Valor")
                        .HasColumnName("man_valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaManutencaoPredialId");

                    b.HasIndex("FornecedorId");

                    b.HasIndex("LocalId");

                    b.HasIndex("StatusId");

                    b.ToTable("tab_manutencao_predial");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.OSGestor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("osg_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataAprovacao")
                        .HasColumnName("osg_data_aprovacao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataConclusao")
                        .HasColumnName("osg_data_conclusao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataSolicitacao")
                        .HasColumnName("osg_data_solicitacao")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EquipamentoId")
                        .HasColumnName("equ_id")
                        .HasColumnType("int");

                    b.Property<int>("FornecedorId")
                        .HasColumnName("for_id")
                        .HasColumnType("int");

                    b.Property<int>("ProblematicaId")
                        .HasColumnName("pro_id")
                        .HasColumnType("int");

                    b.Property<int>("StatusId")
                        .HasColumnName("sta_id")
                        .HasColumnType("int");

                    b.Property<double>("Valor")
                        .HasColumnName("osg_valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("EquipamentoId");

                    b.HasIndex("FornecedorId");

                    b.HasIndex("ProblematicaId");

                    b.HasIndex("StatusId");

                    b.ToTable("tab_os_gestor");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Problematica", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("pro_id")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnName("cat_id")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnName("pro_descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("tab_problematica");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.SaveToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("sav_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnName("sav_data")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnName("sav_email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Token")
                        .HasColumnName("sav_token")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("tab_save_token");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("sta_id")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnName("sta_descricao")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_status");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("usu_id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("usu_email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("Gestor")
                        .HasColumnName("usu_gestor")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("usu_nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Rm")
                        .IsRequired()
                        .HasColumnName("usu_rm")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnName("usu_senha")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("tab_usuario");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.UsuarioEmpresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("usu_emp_id")
                        .HasColumnType("int");

                    b.Property<int>("EmpresaId")
                        .HasColumnName("emp_id")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnName("usu_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("tab_usuario_empresa");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Verbas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ver_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("Periodo")
                        .HasColumnName("ver_periodo")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Valor")
                        .HasColumnName("ver_valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("tab_verba_mensal");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.BlocoEmpresa", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Bloco", "Bloco")
                        .WithMany("BlocoEmpresa")
                        .HasForeignKey("BlocoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.CategoriaEmpresa", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.ContaMensal", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.CategoriaContaMensal", "CategoriaContaMensal")
                        .WithMany("ContaMensal")
                        .HasForeignKey("CategoriaContaMensalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Empresa", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Endereco", "Endereco")
                        .WithMany("Empresas")
                        .HasForeignKey("EnderecoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Equipamento", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Bloco", "Bloco")
                        .WithMany("Equipamentos")
                        .HasForeignKey("BlocoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Categoria", "Categoria")
                        .WithMany("Equipamentos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Local", "Local")
                        .WithMany("Equipamentos")
                        .HasForeignKey("LocalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Fornecedor", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Categoria", null)
                        .WithMany("Fornecedors")
                        .HasForeignKey("CategoriaId");
                });

            modelBuilder.Entity("Fatec_Facilities.Models.FornecedorCategoria", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Fornecedor", "Fornecedor")
                        .WithMany()
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Local", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Bloco", "Bloco")
                        .WithMany("locais")
                        .HasForeignKey("BlocoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.ManutencaoPredial", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.CategoriaManutencaoPredial", "CategoriaManutencaoPredial")
                        .WithMany("ManutencaoPredial")
                        .HasForeignKey("CategoriaManutencaoPredialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Fornecedor", "Fornecedor")
                        .WithMany("ManutencaoPredial")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Local", "Local")
                        .WithMany()
                        .HasForeignKey("LocalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.OSGestor", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Equipamento", "Equipamento")
                        .WithMany("OSGestores")
                        .HasForeignKey("EquipamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Fornecedor", "Fornecedor")
                        .WithMany("OSGestores")
                        .HasForeignKey("FornecedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Problematica", "Problematica")
                        .WithMany("OSGestores")
                        .HasForeignKey("ProblematicaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Status", "Status")
                        .WithMany("OSGestores")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.Problematica", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Categoria", "Categoria")
                        .WithMany("Problematicas")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Fatec_Facilities.Models.UsuarioEmpresa", b =>
                {
                    b.HasOne("Fatec_Facilities.Models.Empresa", "Empresa")
                        .WithMany("UsuarioEmpresa")
                        .HasForeignKey("EmpresaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fatec_Facilities.Models.Usuario", "Usuario")
                        .WithMany("UsuarioEmpresa")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}