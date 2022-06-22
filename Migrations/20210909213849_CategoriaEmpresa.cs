using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fatec_Facilities.Migrations
{
    public partial class CategoriaEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_bloco",
                columns: table => new
                {
                    blo_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    blo_nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_bloco", x => x.blo_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_categoria",
                columns: table => new
                {
                    cat_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cat_nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria", x => x.cat_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_categoria_conta_mensal",
                columns: table => new
                {
                    ccm_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ccm_descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria_conta_mensal", x => x.ccm_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_categoria_manutencao_predial",
                columns: table => new
                {
                    cmp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cmp_nome = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria_manutencao_predial", x => x.cmp_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_endereco",
                columns: table => new
                {
                    end_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    end_logradouro = table.Column<string>(nullable: true),
                    end_numero = table.Column<string>(nullable: true),
                    end_bairro = table.Column<string>(nullable: true),
                    end_cep = table.Column<string>(nullable: true),
                    end_complemento = table.Column<string>(nullable: true),
                    end_cidade = table.Column<string>(nullable: true),
                    end_estado = table.Column<string>(nullable: true),
                    end_pais = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_endereco", x => x.end_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_limite_gastos",
                columns: table => new
                {
                    lim_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    lim_limite_anual = table.Column<double>(nullable: false),
                    lim_ano = table.Column<DateTime>(nullable: false),
                    lim_data_criacao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_limite_gastos", x => x.lim_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_save_token",
                columns: table => new
                {
                    sav_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sav_email = table.Column<string>(nullable: true),
                    sav_token = table.Column<int>(nullable: false),
                    sav_data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_save_token", x => x.sav_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_status",
                columns: table => new
                {
                    sta_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sta_descricao = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_status", x => x.sta_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_usuario",
                columns: table => new
                {
                    usu_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usu_nome = table.Column<string>(nullable: false),
                    usu_email = table.Column<string>(nullable: false),
                    usu_senha = table.Column<string>(nullable: false),
                    usu_rm = table.Column<string>(nullable: false),
                    usu_gestor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_usuario", x => x.usu_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_verba_mensal",
                columns: table => new
                {
                    ver_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ver_valor = table.Column<double>(nullable: false),
                    ver_periodo = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_verba_mensal", x => x.ver_id);
                });

            migrationBuilder.CreateTable(
                name: "tab_local",
                columns: table => new
                {
                    loc_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    loc_nome = table.Column<string>(nullable: false),
                    blo_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_local", x => x.loc_id);
                    table.ForeignKey(
                        name: "FK_tab_local_tab_bloco_blo_id",
                        column: x => x.blo_id,
                        principalTable: "tab_bloco",
                        principalColumn: "blo_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_fornecedor",
                columns: table => new
                {
                    for_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    for_cnpj = table.Column<string>(nullable: false),
                    for_nome = table.Column<string>(nullable: false),
                    for_endereco = table.Column<string>(nullable: false),
                    for_contato = table.Column<string>(nullable: false),
                    for_prestador = table.Column<bool>(nullable: false),
                    CategoriaId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_fornecedor", x => x.for_id);
                    table.ForeignKey(
                        name: "FK_tab_fornecedor_tab_categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "tab_categoria",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tab_problematica",
                columns: table => new
                {
                    pro_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    pro_descricao = table.Column<string>(nullable: false),
                    cat_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_problematica", x => x.pro_id);
                    table.ForeignKey(
                        name: "FK_tab_problematica_tab_categoria_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tab_categoria",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_conta_mensal",
                columns: table => new
                {
                    con_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ccm_id = table.Column<int>(nullable: false),
                    con_valor = table.Column<double>(nullable: false),
                    con_data_vencimento = table.Column<DateTime>(nullable: false),
                    con_date_pagamento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_conta_mensal", x => x.con_id);
                    table.ForeignKey(
                        name: "FK_tab_conta_mensal_tab_categoria_conta_mensal_ccm_id",
                        column: x => x.ccm_id,
                        principalTable: "tab_categoria_conta_mensal",
                        principalColumn: "ccm_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_empresa",
                columns: table => new
                {
                    emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    emp_nome = table.Column<string>(nullable: true),
                    emp_nome_fantasia = table.Column<string>(nullable: true),
                    emp_cnpj = table.Column<string>(nullable: true),
                    end_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_empresa", x => x.emp_id);
                    table.ForeignKey(
                        name: "FK_tab_empresa_tab_endereco_end_id",
                        column: x => x.end_id,
                        principalTable: "tab_endereco",
                        principalColumn: "end_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_equipamento",
                columns: table => new
                {
                    equ_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    equ_registro = table.Column<string>(nullable: false),
                    equ_descricao = table.Column<string>(nullable: false),
                    equ_ativo = table.Column<bool>(nullable: false),
                    cat_id = table.Column<int>(nullable: false),
                    blo_id = table.Column<int>(nullable: false),
                    loc_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_equipamento", x => x.equ_id);
                    table.ForeignKey(
                        name: "FK_tab_equipamento_tab_bloco_blo_id",
                        column: x => x.blo_id,
                        principalTable: "tab_bloco",
                        principalColumn: "blo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_equipamento_tab_categoria_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tab_categoria",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_equipamento_tab_local_loc_id",
                        column: x => x.loc_id,
                        principalTable: "tab_local",
                        principalColumn: "loc_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_fornecedor_categoria",
                columns: table => new
                {
                    for_cat_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    for_id = table.Column<int>(nullable: false),
                    cat_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_fornecedor_categoria", x => x.for_cat_id);
                    table.ForeignKey(
                        name: "FK_tab_fornecedor_categoria_tab_categoria_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tab_categoria",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_fornecedor_categoria_tab_fornecedor_for_id",
                        column: x => x.for_id,
                        principalTable: "tab_fornecedor",
                        principalColumn: "for_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_manutencao_predial",
                columns: table => new
                {
                    man_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    man_descricao = table.Column<string>(nullable: true),
                    loc_id = table.Column<int>(nullable: false),
                    cmp_id = table.Column<int>(nullable: false),
                    man_data_solicitacao = table.Column<DateTime>(nullable: false),
                    man_data_aprovacao = table.Column<DateTime>(nullable: true),
                    man_data_conclusao = table.Column<DateTime>(nullable: true),
                    sta_id = table.Column<int>(nullable: false),
                    for_id = table.Column<int>(nullable: false),
                    man_valor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_manutencao_predial", x => x.man_id);
                    table.ForeignKey(
                        name: "FK_tab_manutencao_predial_tab_categoria_manutencao_predial_cmp_~",
                        column: x => x.cmp_id,
                        principalTable: "tab_categoria_manutencao_predial",
                        principalColumn: "cmp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_manutencao_predial_tab_fornecedor_for_id",
                        column: x => x.for_id,
                        principalTable: "tab_fornecedor",
                        principalColumn: "for_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_manutencao_predial_tab_local_loc_id",
                        column: x => x.loc_id,
                        principalTable: "tab_local",
                        principalColumn: "loc_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_manutencao_predial_tab_status_sta_id",
                        column: x => x.sta_id,
                        principalTable: "tab_status",
                        principalColumn: "sta_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_bloco_empresa",
                columns: table => new
                {
                    blo_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    blo_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_bloco_empresa", x => x.blo_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_bloco_empresa_tab_bloco_blo_id",
                        column: x => x.blo_id,
                        principalTable: "tab_bloco",
                        principalColumn: "blo_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_bloco_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_categoria_empresa",
                columns: table => new
                {
                    cat_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cat_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria_empresa", x => x.cat_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_categoria_empresa_tab_categoria_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tab_categoria",
                        principalColumn: "cat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_categoria_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_usuario_empresa",
                columns: table => new
                {
                    usu_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    usu_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_usuario_empresa", x => x.usu_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_usuario_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_usuario_empresa_tab_usuario_usu_id",
                        column: x => x.usu_id,
                        principalTable: "tab_usuario",
                        principalColumn: "usu_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_os_gestor",
                columns: table => new
                {
                    osg_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    osg_data_solicitacao = table.Column<DateTime>(nullable: false),
                    osg_data_aprovacao = table.Column<DateTime>(nullable: false),
                    osg_data_conclusao = table.Column<DateTime>(nullable: true),
                    equ_id = table.Column<int>(nullable: false),
                    pro_id = table.Column<int>(nullable: false),
                    sta_id = table.Column<int>(nullable: false),
                    for_id = table.Column<int>(nullable: false),
                    osg_valor = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_os_gestor", x => x.osg_id);
                    table.ForeignKey(
                        name: "FK_tab_os_gestor_tab_equipamento_equ_id",
                        column: x => x.equ_id,
                        principalTable: "tab_equipamento",
                        principalColumn: "equ_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_os_gestor_tab_fornecedor_for_id",
                        column: x => x.for_id,
                        principalTable: "tab_fornecedor",
                        principalColumn: "for_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_os_gestor_tab_problematica_pro_id",
                        column: x => x.pro_id,
                        principalTable: "tab_problematica",
                        principalColumn: "pro_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_os_gestor_tab_status_sta_id",
                        column: x => x.sta_id,
                        principalTable: "tab_status",
                        principalColumn: "sta_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tab_bloco_empresa_blo_id",
                table: "tab_bloco_empresa",
                column: "blo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_bloco_empresa_emp_id",
                table: "tab_bloco_empresa",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_empresa_cat_id",
                table: "tab_categoria_empresa",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_empresa_emp_id",
                table: "tab_categoria_empresa",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_conta_mensal_ccm_id",
                table: "tab_conta_mensal",
                column: "ccm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_empresa_end_id",
                table: "tab_empresa",
                column: "end_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_equipamento_blo_id",
                table: "tab_equipamento",
                column: "blo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_equipamento_cat_id",
                table: "tab_equipamento",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_equipamento_loc_id",
                table: "tab_equipamento",
                column: "loc_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_fornecedor_CategoriaId",
                table: "tab_fornecedor",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_tab_fornecedor_categoria_cat_id",
                table: "tab_fornecedor_categoria",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_fornecedor_categoria_for_id",
                table: "tab_fornecedor_categoria",
                column: "for_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_local_blo_id",
                table: "tab_local",
                column: "blo_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_manutencao_predial_cmp_id",
                table: "tab_manutencao_predial",
                column: "cmp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_manutencao_predial_for_id",
                table: "tab_manutencao_predial",
                column: "for_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_manutencao_predial_loc_id",
                table: "tab_manutencao_predial",
                column: "loc_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_manutencao_predial_sta_id",
                table: "tab_manutencao_predial",
                column: "sta_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_os_gestor_equ_id",
                table: "tab_os_gestor",
                column: "equ_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_os_gestor_for_id",
                table: "tab_os_gestor",
                column: "for_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_os_gestor_pro_id",
                table: "tab_os_gestor",
                column: "pro_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_os_gestor_sta_id",
                table: "tab_os_gestor",
                column: "sta_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_problematica_cat_id",
                table: "tab_problematica",
                column: "cat_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_usuario_empresa_emp_id",
                table: "tab_usuario_empresa",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_usuario_empresa_usu_id",
                table: "tab_usuario_empresa",
                column: "usu_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_bloco_empresa");

            migrationBuilder.DropTable(
                name: "tab_categoria_empresa");

            migrationBuilder.DropTable(
                name: "tab_conta_mensal");

            migrationBuilder.DropTable(
                name: "tab_fornecedor_categoria");

            migrationBuilder.DropTable(
                name: "tab_limite_gastos");

            migrationBuilder.DropTable(
                name: "tab_manutencao_predial");

            migrationBuilder.DropTable(
                name: "tab_os_gestor");

            migrationBuilder.DropTable(
                name: "tab_save_token");

            migrationBuilder.DropTable(
                name: "tab_usuario_empresa");

            migrationBuilder.DropTable(
                name: "tab_verba_mensal");

            migrationBuilder.DropTable(
                name: "tab_categoria_conta_mensal");

            migrationBuilder.DropTable(
                name: "tab_categoria_manutencao_predial");

            migrationBuilder.DropTable(
                name: "tab_equipamento");

            migrationBuilder.DropTable(
                name: "tab_fornecedor");

            migrationBuilder.DropTable(
                name: "tab_problematica");

            migrationBuilder.DropTable(
                name: "tab_status");

            migrationBuilder.DropTable(
                name: "tab_empresa");

            migrationBuilder.DropTable(
                name: "tab_usuario");

            migrationBuilder.DropTable(
                name: "tab_local");

            migrationBuilder.DropTable(
                name: "tab_categoria");

            migrationBuilder.DropTable(
                name: "tab_endereco");

            migrationBuilder.DropTable(
                name: "tab_bloco");
        }
    }
}
