using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyAndUpdateAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "account");

            migrationBuilder.AddColumn<Guid>(
                name: "CurrencyId",
                table: "account",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "currency",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_CurrencyId",
                table: "account",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_account_currency_CurrencyId",
                table: "account",
                column: "CurrencyId",
                principalTable: "currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_account_currency_CurrencyId",
                table: "account");

            migrationBuilder.DropTable(
                name: "currency");

            migrationBuilder.DropIndex(
                name: "IX_account_CurrencyId",
                table: "account");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "account");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "account",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }
    }
}
