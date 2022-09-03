using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using WatermarkMQ.Contexts;
using WatermarkMQ.Implemantations;
using WatermarkMQ.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(dbOpts =>
{
    dbOpts.UseInMemoryDatabase("ProductDB");
});

builder.Services.AddSingleton(new ConnectionFactory()
{
    Uri = new Uri(Configuration.GetConnectionString("RabbitMQ"))
});
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
