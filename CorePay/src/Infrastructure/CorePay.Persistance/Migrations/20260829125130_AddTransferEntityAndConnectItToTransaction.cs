using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorePay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddTransferEntityAndConnectItToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TransferId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SenderAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecieverAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecieverCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_RecieverAccountId",
                        column: x => x.RecieverAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Accounts_SenderAccountId",
                        column: x => x.SenderAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Cards_RecieverCardId",
                        column: x => x.RecieverCardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_Cards_SenderCardId",
                        column: x => x.SenderCardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TransferId",
                table: "Transactions",
                column: "TransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecieverAccountId",
                table: "Transfers",
                column: "RecieverAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_RecieverCardId",
                table: "Transfers",
                column: "RecieverCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderAccountId",
                table: "Transfers",
                column: "SenderAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SenderCardId",
                table: "Transfers",
                column: "SenderCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transfers_TransferId",
                table: "Transactions",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transfers_TransferId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_TransferId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransferId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "AccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
