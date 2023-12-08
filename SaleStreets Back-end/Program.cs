using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaleStreets_Back_end.Configurations;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Services;
using SaleStreets_Back_end.Services.Authorization;
using SaleStreets_Back_end.Services.Products;
using SaleStreets_Back_end.Services.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.OperationFilter<FileOperationFilter>();
});

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddScoped<IAuthService , AuthService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IImageService , ImageService>();
builder.Services.AddScoped<IProductService, ProductService>(); 

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;

});


builder.Services.AddIdentity<AppUser , IdentityRole>(
    options =>
    {
        // for testing purposes only
        options.User.RequireUniqueEmail= true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
    }
    ).AddEntityFrameworkStores<ApplicationDbConext>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbConext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{

   var validationParams = new TokenValidationParameters()
   {
       IssuerSigningKey = new SymmetricSecurityKey(Encoding.
       UTF8.GetBytes(builder.Configuration["JWT:Secret"])) , 
       ValidateIssuerSigningKey = true ,
       ValidateLifetime= true ,
       RequireExpirationTime= true ,
       ClockSkew = TimeSpan.Zero,
       RequireAudience = false ,   
       ValidateAudience= false ,
       ValidateIssuer = false ,
       
   } ;
    

    // only in development
    options.RequireHttpsMetadata= false;
    options.TokenValidationParameters= validationParams;
}); 
var app = builder.Build();

app.UseCors(o =>
{
    o.AllowAnyHeader();
    o.AllowAnyMethod();
    o.AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
