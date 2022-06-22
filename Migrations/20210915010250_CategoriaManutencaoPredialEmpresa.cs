using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fatec_Facilities.Migrations
{
    public partial class CategoriaManutencaoPredialEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_categoria_conta_mensal_empresa",
                columns: table => new
                {
                    ccm_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ccm_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria_conta_mensal_empresa", x => x.ccm_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_categoria_conta_mensal_empresa_tab_categoria_conta_mensa~",
                        column: x => x.ccm_id,
                        principalTable: "tab_categoria_conta_mensal",
                        principalColumn: "ccm_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_categoria_conta_mensal_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tab_categoria_manutencao_predial_empresa",
                columns: table => new
                {
                    cmp_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    cmp_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_categoria_manutencao_predial_empresa", x => x.cmp_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_categoria_manutencao_predial_empresa_tab_categoria_manut~",
                        column: x => x.cmp_id,
                        principalTable: "tab_categoria_manutencao_predial",
                        principalColumn: "cmp_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_categoria_manutencao_predial_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_conta_mensal_empresa_ccm_id",
                table: "tab_categoria_conta_mensal_empresa",
                column: "ccm_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_conta_mensal_empresa_emp_id",
                table: "tab_categoria_conta_mensal_empresa",
                column: "emp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_manutencao_predial_empresa_cmp_id",
                table: "tab_categoria_manutencao_predial_empresa",
                column: "cmp_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_categoria_manutencao_predial_empresa_emp_id",
                table: "tab_categoria_manutencao_predial_empresa",
                column: "emp_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_categoria_conta_mensal_empresa");

            migrationBuilder.DropTable(
                name: "tab_categoria_manutencao_predial_empresa");
        }
    }
}
