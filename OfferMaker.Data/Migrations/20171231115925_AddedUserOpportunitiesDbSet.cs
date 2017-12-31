namespace OfferMaker.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;
    using System.Collections.Generic;

    public partial class AddedUserOpportunitiesDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOpportunity_Opportunities_OpportunityId",
                table: "UserOpportunity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOpportunity_AspNetUsers_UserId",
                table: "UserOpportunity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOpportunity",
                table: "UserOpportunity");

            migrationBuilder.RenameTable(
                name: "UserOpportunity",
                newName: "UserOpportunities");

            migrationBuilder.RenameIndex(
                name: "IX_UserOpportunity_OpportunityId",
                table: "UserOpportunities",
                newName: "IX_UserOpportunities_OpportunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOpportunities",
                table: "UserOpportunities",
                columns: new[] { "UserId", "OpportunityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOpportunities_Opportunities_OpportunityId",
                table: "UserOpportunities",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOpportunities_AspNetUsers_UserId",
                table: "UserOpportunities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOpportunities_Opportunities_OpportunityId",
                table: "UserOpportunities");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOpportunities_AspNetUsers_UserId",
                table: "UserOpportunities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOpportunities",
                table: "UserOpportunities");

            migrationBuilder.RenameTable(
                name: "UserOpportunities",
                newName: "UserOpportunity");

            migrationBuilder.RenameIndex(
                name: "IX_UserOpportunities_OpportunityId",
                table: "UserOpportunity",
                newName: "IX_UserOpportunity_OpportunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOpportunity",
                table: "UserOpportunity",
                columns: new[] { "UserId", "OpportunityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOpportunity_Opportunities_OpportunityId",
                table: "UserOpportunity",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOpportunity_AspNetUsers_UserId",
                table: "UserOpportunity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
