using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorePay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddPINHashPropToCardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "PinHash",
                table: "Cards",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transaction_AccountId_Or_CardId_Required",
                table: "Transactions",
                sql: "[AccountId] IS NOT NULL OR [CardId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Transaction_AccountId_Or_CardId_Required",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "PinHash",
                table: "Cards");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
