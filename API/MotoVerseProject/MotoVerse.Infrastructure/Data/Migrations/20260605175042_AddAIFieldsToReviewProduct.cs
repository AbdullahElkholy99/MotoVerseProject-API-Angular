using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoVerse.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAIFieldsToReviewProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminAutoReply",
                table: "ReviewProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AnalyzedAt",
                table: "ReviewProducts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FakeReason",
                table: "ReviewProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFake",
                table: "ReviewProducts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sentiment",
                table: "ReviewProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SentimentScore",
                table: "ReviewProducts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminAutoReply",
                table: "ReviewProducts");

            migrationBuilder.DropColumn(
                name: "AnalyzedAt",
                table: "ReviewProducts");

            migrationBuilder.DropColumn(
                name: "FakeReason",
                table: "ReviewProducts");

            migrationBuilder.DropColumn(
                name: "IsFake",
                table: "ReviewProducts");

            migrationBuilder.DropColumn(
                name: "Sentiment",
                table: "ReviewProducts");

            migrationBuilder.DropColumn(
                name: "SentimentScore",
                table: "ReviewProducts");
        }
    }
}
