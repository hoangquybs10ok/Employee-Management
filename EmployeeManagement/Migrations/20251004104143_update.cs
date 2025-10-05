using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Users_UserEntityId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UserEntityId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserEntityId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UserEntityId",
                table: "Projects",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Users_UserEntityId",
                table: "Projects",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
