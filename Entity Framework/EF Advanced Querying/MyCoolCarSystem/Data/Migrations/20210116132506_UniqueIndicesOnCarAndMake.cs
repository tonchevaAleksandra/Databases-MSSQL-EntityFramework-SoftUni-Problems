using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCoolCarSystem.Data.Migrations
{
    public partial class UniqueIndicesOnCarAndMake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Makes_Name",
                table: "Makes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_Vin",
                table: "Cars",
                column: "Vin",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Makes_Name",
                table: "Makes");

            migrationBuilder.DropIndex(
                name: "IX_Cars_Vin",
                table: "Cars");
        }
    }
}
