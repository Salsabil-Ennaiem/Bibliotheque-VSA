using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MdfDateFinSanc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_fin_sanction",
                table: "Sanction",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_emp",
                table: "Emprunts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 28, 15, 4, 15, 247, DateTimeKind.Utc).AddTicks(8933),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 5, 28, 11, 16, 8, 294, DateTimeKind.Utc).AddTicks(9027));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_fin_sanction",
                table: "Sanction",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_emp",
                table: "Emprunts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 5, 28, 11, 16, 8, 294, DateTimeKind.Utc).AddTicks(9027),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2025, 5, 28, 15, 4, 15, 247, DateTimeKind.Utc).AddTicks(8933));
        }
    }
}
