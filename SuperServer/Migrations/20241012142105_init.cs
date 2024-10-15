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
                    DBHeroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    HeroName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Class = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Gold = table.Column<int>(type: "int", nullable: false),
                    Exp = table.Column<int>(type: "int", nullable: false),
                    HeroStat_MaxHp = table.Column<int>(type: "int", nullable: true),
                    HeroStat_MaxMp = table.Column<int>(type: "int", nullable: true),
                    HeroStat_HP = table.Column<int>(type: "int", nullable: true),
                    HeroStat_MP = table.Column<int>(type: "int", nullable: true),
                    HeroStat_AtkDamage = table.Column<int>(type: "int", nullable: true),
                    HeroStat_Defense = table.Column<int>(type: "int", nullable: true),
                    HeroStat_MoveSpeed = table.Column<float>(type: "real", nullable: true),
                    HeroStat_AtkSpeed = table.Column<float>(type: "real", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    PosX = table.Column<float>(type: "real", nullable: false),
                    PosY = table.Column<float>(type: "real", nullable: false),
                    PosZ = table.Column<float>(type: "real", nullable: false),
                    RotY = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hero", x => x.DBHeroId);
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
