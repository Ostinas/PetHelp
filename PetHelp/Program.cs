using Microsoft.EntityFrameworkCore;
using PetHelp.Data;
using PetHelp.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PetHelpContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PetHelpContext") ?? throw new InvalidOperationException("Connection string 'PetHelpContext' not found.")));

// Add services to the container.

builder.Services.AddTransient<PetRepository>();
builder.Services.AddTransient<AdRepository>();
builder.Services.AddTransient<OwnerRepository>();
builder.Services.AddTransient<ApplicantRepository>();


builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PetHelpAPI");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
