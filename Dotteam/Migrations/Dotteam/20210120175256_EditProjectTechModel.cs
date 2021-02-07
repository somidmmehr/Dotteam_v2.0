using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotteam.Migrations.Dotteam
{
    public partial class EditProjectTechModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTechModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechModel",
                table: "TechModel");

            migrationBuilder.RenameTable(
                name: "TechModel",
                newName: "Techs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Techs",
                table: "Techs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProjectModelTechModel",
                columns: table => new
                {
                    ProjectsId = table.Column<int>(type: "int", nullable: false),
                    TechesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectModelTechModel", x => new { x.ProjectsId, x.TechesId });
                    table.ForeignKey(
                        name: "FK_ProjectModelTechModel_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectModelTechModel_Techs_TechesId",
                        column: x => x.TechesId,
                        principalTable: "Techs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectModelTechModel_TechesId",
                table: "ProjectModelTechModel",
                column: "TechesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectModelTechModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Techs",
                table: "Techs");

            migrationBuilder.RenameTable(
                name: "Techs",
                newName: "TechModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechModel",
                table: "TechModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProjectTechModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    TechId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTechModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectTechModel_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTechModel_TechModel_TechId",
                        column: x => x.TechId,
                        principalTable: "TechModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechModel_ProjectId",
                table: "ProjectTechModel",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechModel_TechId",
                table: "ProjectTechModel",
                column: "TechId");
        }
    }
}
