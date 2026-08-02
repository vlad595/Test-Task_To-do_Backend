using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Data;
using Service;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAngularApp", policy =>
   {
       policy.WithOrigins("https://localhost:4200").AllowAnyHeader().AllowAnyMethod();
   });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
           Type = SecuritySchemeType.Http,
           Scheme = "bearer",
           BearerFormat = "JWT",
           Description = "Enter only JWT-token"
        });

        c.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", doc, null)] = []
        });
}); 

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SecondConnection")));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");
app.MapGet("/", () => "Hello World!");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
