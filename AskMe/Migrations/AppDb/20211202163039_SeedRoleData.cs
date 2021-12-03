using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AskMe.Migrations.AppDb
{
    public partial class SeedRoleData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "84651344-d6a2-43b5-8be8-8c4103a2ea41", "442ba41c-79cc-4c93-beae-c218a7adb03e", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2601c87-2424-4b0f-a3ea-f0fbf9a4f4c0", "fbf91f61-2da4-45f3-a1d6-740da68e0ec1", "Consumer", "CONSUMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84651344-d6a2-43b5-8be8-8c4103a2ea41");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2601c87-2424-4b0f-a3ea-f0fbf9a4f4c0");
        }
    }
}
