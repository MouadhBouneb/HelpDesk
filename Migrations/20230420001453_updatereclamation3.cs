using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HelpDesk4GL.Migrations
{
    /// <inheritdoc />
    public partial class updatereclamation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reclamations_User_UserId",
                table: "Reclamations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reclamations",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Reclamations_User_UserId",
                table: "Reclamations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reclamations_User_UserId",
                table: "Reclamations");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Reclamations",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reclamations_User_UserId",
                table: "Reclamations",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
