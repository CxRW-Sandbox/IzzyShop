using Microsoft.EntityFrameworkCore;
using IzzyShop.Data;

var builder = WebApplication.CreateBuilder(args);

// Vulnerable: CORS configuration too permissive
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Vulnerable: No HTTPS redirection
builder.Services.AddControllers();

// Vulnerable: No proper security headers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Vulnerable: No proper database security
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Vulnerable: Development error page in production
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Vulnerable: No proper security headers
app.UseHttpsRedirection();

// Vulnerable: CORS policy too permissive
app.UseCors("AllowAll");

// Vulnerable: No proper authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run(); 