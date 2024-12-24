using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperServer.Migrations
{
    public partial class restoreSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "Hero",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Skills",
                table: "Hero");
        }
    }
}
