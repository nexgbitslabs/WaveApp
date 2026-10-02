using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Profiles_LoginId",
                table: "Profiles",
                column: "LoginId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Logins_LoginId",
                table: "Profiles",
                column: "LoginId",
                principalTable: "Logins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Logins_LoginId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_LoginId",
                table: "Profiles");
        }
    }
}
