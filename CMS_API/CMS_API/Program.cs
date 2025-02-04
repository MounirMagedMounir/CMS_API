using CMS_API.Infrastructure;
using CMS_API.Middleware;
using CMS_API.Services;
using CMS_API.Services.Article;
using CMS_API.Services.Authentication;
using CMS_API.Services.Authorization;
using CMS_API_Application.Interfaces.Servises;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Application.Interfaces.Servises.Authentication;
using CMS_API_Application.Interfaces.Servises.Authorization;
using CMS_API_Application.Services.Article;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Core.Interfaces.Repository.Authentication;
using CMS_API_Core.Interfaces.Repository.Authorization;
using CMS_API_Infrastructure.DBcontext;
using CMS_API_Infrastructure.Repository;
using CMS_API_Infrastructure.Repository.Article;
using CMS_API_Infrastructure.Repository.Authentication;
using CMS_API_Infrastructure.Repository.Authorization;
using CMS_API_Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllClients", policy =>
    {
        policy.WithOrigins(
                "https://localhost:7265", // Blazor
                "http://localhost:4200",  // Angular local
                "https://mounirmagedmounir.github.io" // Deployed Angular
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("CMS_API_Infrastructure")));

builder.Services.AddScoped<DataContext>();

builder.Services.AddScoped<IAuthUserService, AuthUserService>();
builder.Services.AddScoped<IAuthAdminService, AuthAdminService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();

builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IArticleContributorRepository, ArticleContributorRepository>();
builder.Services.AddScoped<ITagArticleRepository, TagArticleRepository>();


builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<ISecurityService, SecurityService>();




var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllClients");

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SessionTokenCheckMiddleware>();
app.UseMiddleware<InActiveUpdateMiddleware>();

app.MapControllers();

app.Run();
