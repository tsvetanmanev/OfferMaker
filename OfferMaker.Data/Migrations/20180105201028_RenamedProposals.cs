using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OfferMaker.Data.Migrations
{
    public partial class RenamedProposals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Opportunities_OpportunityId",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.RenameTable(
                name: "Offers",
                newName: "Proposals");

            migrationBuilder.RenameIndex(
                name: "IX_Offers_OpportunityId",
                table: "Proposals",
                newName: "IX_Proposals_OpportunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proposals",
                table: "Proposals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proposals_Opportunities_OpportunityId",
                table: "Proposals",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proposals_Opportunities_OpportunityId",
                table: "Proposals");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proposals",
                table: "Proposals");

            migrationBuilder.RenameTable(
                name: "Proposals",
                newName: "Offers");

            migrationBuilder.RenameIndex(
                name: "IX_Proposals_OpportunityId",
                table: "Offers",
                newName: "IX_Offers_OpportunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Opportunities_OpportunityId",
                table: "Offers",
                column: "OpportunityId",
                principalTable: "Opportunities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
