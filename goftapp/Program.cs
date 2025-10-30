using System.Text.Json.Serialization;
using goftapp.Components;
using goftapp.Infrastructure;
using goftapp.Services;
using goftapp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();

// DI
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IDailyLogService, DailyLogService>();


builder.Services.AddOpenApi();

var app = builder.Build();

// seed admin
await DbSeeder.SeedAdminAsync(app.Services);

app.MapOpenApi();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapBlazorHub();
app.MapStaticAssets();
app.MapFallbackToPage("/_Host");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
