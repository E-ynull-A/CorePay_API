using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorePay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInInvitationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInvitations_AspNetUsers_CreatedUserId",
                table: "GroupInvitations");

            migrationBuilder.RenameColumn(
                name: "CreatedUserId",
                table: "GroupInvitations",
                newName: "InvitedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupInvitations_CreatedUserId",
                table: "GroupInvitations",
                newName: "IX_GroupInvitations_InvitedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInvitations_AspNetUsers_InvitedUserId",
                table: "GroupInvitations",
                column: "InvitedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupInvitations_AspNetUsers_InvitedUserId",
                table: "GroupInvitations");

            migrationBuilder.RenameColumn(
                name: "InvitedUserId",
                table: "GroupInvitations",
                newName: "CreatedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupInvitations_InvitedUserId",
                table: "GroupInvitations",
                newName: "IX_GroupInvitations_CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupInvitations_AspNetUsers_CreatedUserId",
                table: "GroupInvitations",
                column: "CreatedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
