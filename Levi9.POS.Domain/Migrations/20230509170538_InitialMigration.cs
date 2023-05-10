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
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230509130003_InitialMigration.cs
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("66632284-53c8-4526-8381-1d10fcc4f5f7"), "133281108035946391", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("981b0758-6807-4676-bdd8-89deaa4f74b1"), "133281108035946425", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("6d3ec195-1691-41fd-863c-5c7d0b869994"), "133281108035946432", "Milos", "password123", "+387 65 912 127", "Salt123" }
========
                    { 1, "1.maja, Derventa", "marko@gmail.com", new Guid("f6f8b863-0268-499b-afab-f4ca25f663e9"), "133281255386347709", "Marko", "password", "+387 65 132 527", "Salt" },
                    { 2, "Koste Racina 24, Novi Sad", "aleksa@gmail.com", new Guid("bb253e8c-6b2a-42df-81b9-114db11fa718"), "133281255386347718", "Aleksa", "password123", "+387 64 862 476", "Salt123" },
                    { 3, "Strumicka 13, Novi Sad", "milos@gmail.com", new Guid("58854395-6000-4c8c-a54a-327aa0bfd2c8"), "133281255386347723", "Milos", "password123", "+387 65 912 127", "Salt123" }
>>>>>>>> main:Levi9.POS.Domain/Migrations/20230509170538_InitialMigration.cs
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AvailableQuantity", "GlobalId", "LastUpdate", "Name", "Price", "ProductImageUrl" },
                values: new object[,]
                {
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230509130003_InitialMigration.cs
                    { 1, 30, new Guid("b4ab648f-6826-4b42-bbe7-d35d7a888512"), "133281108035946177", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("704c2aae-8556-40f4-8d49-bdc854f6a930"), "133281108035946257", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("e599ac11-2627-484d-8510-ca29a9bdc323"), "133281108035946266", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
========
                    { 1, 30, new Guid("5f99a16d-e789-4bfb-9623-e3dc29a3b8d0"), "133281255386347547", "Levi 9 T-Shirt", 10f, "baseURL//nekiurl1.png" },
                    { 2, 10, new Guid("15b57178-fa2d-4748-87ca-18669139b6ed"), "133281255386347605", "Novis T-Shirt", 15f, "baseURL//nekiurl2.png" },
                    { 3, 20, new Guid("8c9783c5-a267-4088-b5f8-738a2da49a88"), "133281255386347622", "Vega IT T-Shirt", 20f, "baseURL//nekiurl3.png" }
>>>>>>>> main:Levi9.POS.Domain/Migrations/20230509170538_InitialMigration.cs
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230509130003_InitialMigration.cs
                values: new object[] { 1, 1, "133281108035946455", "INVOICE", new Guid("939b5eb6-c8be-40f7-b50b-7c5983fd9f22"), "133281108035946464" });
========
                values: new object[] { 1, 1, "133281255386347739", "INVOICE", new Guid("67bead56-76b8-4082-95e8-c04a2732eb5a"), "133281255386347744" });
>>>>>>>> main:Levi9.POS.Domain/Migrations/20230509170538_InitialMigration.cs

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230509130003_InitialMigration.cs
                values: new object[] { 2, 2, "133281108035946471", "INVOICE", new Guid("b7697653-aecb-4582-a744-22baa2e2713e"), "133281108035946476" });
========
                values: new object[] { 2, 2, "133281255386347749", "INVOICE", new Guid("e684a4ea-62f3-4f87-88cc-bdc389c9bcc1"), "133281255386347753" });
>>>>>>>> main:Levi9.POS.Domain/Migrations/20230509170538_InitialMigration.cs

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "ClientId", "CreationDay", "DocumetType", "GlobalId", "LastUpdate" },
<<<<<<<< HEAD:Levi9.POS.Domain/Migrations/20230509130003_InitialMigration.cs
                values: new object[] { 3, 3, "133281108035946482", "INVOICE", new Guid("fd88be6a-048f-442c-970a-fe8856d5b2a4"), "133281108035946487" });
========
                values: new object[] { 3, 3, "133281255386347758", "INVOICE", new Guid("f3e4739a-e4e0-4fb3-bab2-198d26531eef"), "133281255386347761" });
>>>>>>>> main:Levi9.POS.Domain/Migrations/20230509170538_InitialMigration.cs

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
