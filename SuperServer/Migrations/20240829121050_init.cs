using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuperServer.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hero",
                columns: table => new
                {
                    HeroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    HeroName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Class = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    HeroStat_MaxHp = table.Column<float>(type: "real", nullable: true),
                    HeroStat_MaxMp = table.Column<float>(type: "real", nullable: true),
                    HeroStat_HP = table.Column<float>(type: "real", nullable: true),
                    HeroStat_MP = table.Column<float>(type: "real", nullable: true),
                    HeroStat_AttackDamage = table.Column<float>(type: "real", nullable: true),
                    HeroStat_Defense = table.Column<float>(type: "real", nullable: true),
                    HeroStat_MoveSpeed = table.Column<float>(type: "real", nullable: true),
                    HeroStat_AtkSpeed = table.Column<float>(type: "real", nullable: true),
                    HeroStat_Exp = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hero", x => x.HeroId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hero_HeroName",
                table: "Hero",
                column: "HeroName",
                unique: true,
                filter: "[HeroName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hero");
        }
    }
}
