using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotteam.Migrations.Dotteam
{
    public partial class UpdatedProjectModel_CreatedProjectTechModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Projects",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionLong",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionShort",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectTechModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTechModel_ProjectId",
                table: "ProjectTechModel",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectTechModel");

            migrationBuilder.DropColumn(
                name: "DescriptionLong",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DescriptionShort",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Projects",
                newName: "Description");
        }
    }
}
