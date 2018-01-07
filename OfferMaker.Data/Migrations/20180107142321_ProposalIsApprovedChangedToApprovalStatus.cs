using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace OfferMaker.Data.Migrations
{
    public partial class ProposalIsApprovedChangedToApprovalStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Proposals");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Proposals",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Proposals");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Proposals",
                nullable: false,
                defaultValue: false);
        }
    }
}
