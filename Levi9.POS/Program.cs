using Levi9.POS.Domain;
using Levi9.POS.Domain.Common;
using Levi9.POS.Domain.Common.IDocument;
using Levi9.POS.Domain.Common.IProduct;
using Levi9.POS.Domain.Repositories;
using Levi9.POS.Domain.Repository;
using Levi9.POS.Domain.Service;
using Levi9.POS.Domain.Services;
using Levi9.POS.WebApi.Mapper;
using Levi9.POS.WebApi.Mappings;
using Levi9.POS.WebApi.Response;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Add DefaultConnection
builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Mappers
builder.Services.AddAutoMapper(typeof(DocumentMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ClientMappingProfile).Assembly);
//Repositories
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
//Services
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProductService, ProductService>();
//Logger
builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var dbContext = services.GetRequiredService<DataBaseContext>();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.MapControllers();

app.Run();
