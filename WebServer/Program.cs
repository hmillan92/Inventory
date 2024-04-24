using Blazored.Toast;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Web.Data;
using WebServer.Data.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<DataContext>(cfg =>
{
    cfg.UseSqlite(connectionString: "Filename=dbSqlite.db",
                sqliteOptionsAction: op =>
                {
                    op.MigrationsAssembly(
                        Assembly.GetExecutingAssembly().FullName);
                });

    //Si queremos utilizar otro motor de BD necesitamos incluir aqui otro nombre, para usar SQL es cfg.UseSQLServer
});
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<IRateServices, RateServices>();
builder.Services.AddBlazoredToast();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
