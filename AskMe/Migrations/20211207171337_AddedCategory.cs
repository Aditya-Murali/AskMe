using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AskMe.Migrations
{
    public partial class AddedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CId",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CId);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CId", "Name" },
                values: new object[,]
                {
                    { 1, "Health" },
                    { 2, "Technology" },
                    { 3, "Automotive" },
                    { 4, "Art" },
                    { 5, "Education" },
                    { 6, "Career" },
                    { 7, "Politics" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CId",
                table: "Questions",
                column: "CId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Categories_CId",
                table: "Questions",
                column: "CId",
                principalTable: "Categories",
                principalColumn: "CId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Categories_CId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CId",
                table: "Questions");
        }
    }
}
