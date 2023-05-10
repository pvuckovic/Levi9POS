using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Levi9.POS.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LastUpdate = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    DocumetType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreationDay = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDocuments",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    DocumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDocuments", x => new { x.ProductId, x.DocumentId });
                    table.ForeignKey(
                        name: "FK_ProductDocuments_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDocuments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Address", "Email", "GlobalId", "LastUpdate", "Name", "PasswordHash", "Phone", "Salt" },
                values: new object[,]
                {
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("e9efe787-01f6-4185-969b-2c270d0ca85d"), "133281953975052071", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("ebcbb4cf-b09d-4363-8c47-aaaf374d8d74"), "133281953975052084", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("4e835e2d-e6e4-423b-91d7-065a04fe9539"), "133281953975052091", "Milos", "password123", "+387 65 912 127", "Salt123" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "GlobalId", "LastUpdate", "Name", "Price", "ProductImageUrl" },
                values: new object[,]
                {
                    { 1, 30, new Guid("8419e5f6-00e6-48e1-9ba6-db7d02455184"), "133281953975051861", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("c4650998-4480-4636-b6fc-bd783bd6e8ee"), "133281953975051935", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("fced7455-8b25-4a78-9aee-89e8cf791f69"), "133281953975051944", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[,]
                {
                    { 1, 1, "133281953975052112", "INVOICE", new Guid("792859e6-1a29-4f2a-ac89-df5a7c32813f"), "133281953975052119" },
                    { 2, 2, "133281953975052126", "INVOICE", new Guid("f750d6cc-b2e4-460b-945c-993df1c069a4"), "133281953975052131" },
                    { 3, 3, "133281953975052137", "INVOICE", new Guid("59b2716d-4a05-4356-9066-cba58bcca141"), "133281953975052143" }
                });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "RSD", 1200f, 20 },
                    { 2, 2, "EUR", 10f, 10 },
                    { 3, 3, "USD", 15f, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ClientId",
                table: "Documents",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDocuments_DocumentId",
                table: "ProductDocuments",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDocuments");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
