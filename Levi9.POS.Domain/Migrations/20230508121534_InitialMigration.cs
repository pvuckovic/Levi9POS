using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Levi9.POS.Domain.Migrations
{
    public partial class InitialMigration : Migration
    {
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
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
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
                columns: new[] { "Id", "Address", "Email", "GlobalId", "LastUpdate", "Name", "Password", "Phone", "Salt" },
                values: new object[,]
                {
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("2c4661b9-897c-4053-80f8-dda5f382857c"), "133280217345321113", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("7443ac60-8b5d-4786-9675-42b6f3c1ff7e"), "133280217345321135", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("86e36676-8c8c-43ed-acf6-582c205d5ba2"), "133280217345321150", "Milos", "password123", "+387 65 912 127", "Salt123" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "GlobalId", "LastUpdate", "Name", "Price", "ProductImageUrl" },
                values: new object[,]
                {
                    { 1, 30, new Guid("d9da34a0-a6ec-4428-8674-c0fca1b43fc1"), "133280217345320737", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("b3f20159-45e9-4a3c-b5e5-725a99e63e6e"), "133280217345320844", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("ca6962b5-7cd3-4709-8328-8878301e0155"), "133280217345320876", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 1, 1, "133280217345321182", "INVOICE", new Guid("f40d84b7-f90b-446a-b3dc-3caf22816436"), "133280217345321193" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 2, 2, "133280217345321205", "INVOICE", new Guid("943546af-6640-4256-9c99-e26f89fcb885"), "133280217345321217" });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
                values: new object[] { 3, 3, "133280217345321231", "INVOICE", new Guid("cb2d859d-59a6-44d8-93d7-059d4649cb77"), "133280217345321242" });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 1, 1, "RSD", 1200f, 20 });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 2, 2, "EUR", 10f, 10 });

            migrationBuilder.InsertData(
                table: "ProductDocuments",
                columns: new[] { "DocumentId", "ProductId", "Currency", "Price", "Quantity" },
                values: new object[] { 3, 3, "USD", 15f, 15 });

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
