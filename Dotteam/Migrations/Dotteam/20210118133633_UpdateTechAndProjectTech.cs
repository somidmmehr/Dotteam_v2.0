using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotteam.Migrations.Dotteam
{
    public partial class UpdateTechAndProjectTech : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectTechModel");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProjectTechModel");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProjectTechModel");

            migrationBuilder.AddColumn<int>(
                name: "TechId",
                table: "ProjectTechModel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TechModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechModel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechModel_TechId",
                table: "ProjectTechModel",
                column: "TechId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTechModel_TechModel_TechId",
                table: "ProjectTechModel",
                column: "TechId",
                principalTable: "TechModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTechModel_TechModel_TechId",
                table: "ProjectTechModel");

            migrationBuilder.DropTable(
                name: "TechModel");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTechModel_TechId",
                table: "ProjectTechModel");

            migrationBuilder.DropColumn(
                name: "TechId",
                table: "ProjectTechModel");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectTechModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProjectTechModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProjectTechModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
