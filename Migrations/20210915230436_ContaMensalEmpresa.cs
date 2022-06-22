using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fatec_Facilities.Migrations
{
    public partial class ContaMensalEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tab_conta_mensal_empresa",
                columns: table => new
                {
                    con_emp_id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    con_id = table.Column<int>(nullable: false),
                    cat_con_id = table.Column<int>(nullable: false),
                    emp_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tab_conta_mensal_empresa", x => x.con_emp_id);
                    table.ForeignKey(
                        name: "FK_tab_conta_mensal_empresa_tab_categoria_conta_mensal_cat_con_~",
                        column: x => x.cat_con_id,
                        principalTable: "tab_categoria_conta_mensal",
                        principalColumn: "ccm_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_conta_mensal_empresa_tab_conta_mensal_con_id",
                        column: x => x.con_id,
                        principalTable: "tab_conta_mensal",
                        principalColumn: "con_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tab_conta_mensal_empresa_tab_empresa_emp_id",
                        column: x => x.emp_id,
                        principalTable: "tab_empresa",
                        principalColumn: "emp_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tab_conta_mensal_empresa_cat_con_id",
                table: "tab_conta_mensal_empresa",
                column: "cat_con_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_conta_mensal_empresa_con_id",
                table: "tab_conta_mensal_empresa",
                column: "con_id");

            migrationBuilder.CreateIndex(
                name: "IX_tab_conta_mensal_empresa_emp_id",
                table: "tab_conta_mensal_empresa",
                column: "emp_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tab_conta_mensal_empresa");
        }
    }
}
