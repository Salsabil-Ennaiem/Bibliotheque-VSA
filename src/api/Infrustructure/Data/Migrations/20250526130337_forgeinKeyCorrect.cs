using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class forgeinKeyCorrect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprunts_Membre_id_emp",
                table: "Emprunts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_emp",
                table: "Emprunts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 26, 13, 3, 35, 822, DateTimeKind.Utc).AddTicks(5940),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 5, 26, 12, 47, 30, 213, DateTimeKind.Utc).AddTicks(8473));

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_id_membre",
                table: "Emprunts",
                column: "id_membre");

            migrationBuilder.AddForeignKey(
                name: "FK_Emprunts_Membre_id_membre",
                table: "Emprunts",
                column: "id_membre",
                principalTable: "Membre",
                principalColumn: "id_membre",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emprunts_Membre_id_membre",
                table: "Emprunts");

            migrationBuilder.DropIndex(
                name: "IX_Emprunts_id_membre",
                table: "Emprunts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_emp",
                table: "Emprunts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 26, 12, 47, 30, 213, DateTimeKind.Utc).AddTicks(8473),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 5, 26, 13, 3, 35, 822, DateTimeKind.Utc).AddTicks(5940));

            migrationBuilder.AddForeignKey(
                name: "FK_Emprunts_Membre_id_emp",
                table: "Emprunts",
                column: "id_emp",
                principalTable: "Membre",
                principalColumn: "id_membre",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
