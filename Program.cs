
 global using Microsoft.EntityFrameworkCore;
 global using MyWebApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyWebApi.Data;
using MyWebApi.Interfaces;
using MyWebApi.Services.CharacterService;
using MyWebApi.Services.FightService;
using MyWebApi.Services.WeaponService;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle///...
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c=>{
    c.AddSecurityDefinition("oauth2",new OpenApiSecurityScheme{
 Description = """Standard Authorization header using the Bearer sceme.Example : "bearer {token}" """,
          In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});
//Weapon Service and Interface;
builder.Services.AddScoped<IWeaponService,WeaponService>();
//Character Service and Interface;
builder.Services.AddScoped<ICharacterService,CharactereService>();
//Authorization and Interface;
builder.Services.AddScoped<IAuthRepository,AuthRepository>();
//fight service and Iterface;
builder.Services.AddScoped<IFightService,FightService>();
//AutoMapper;
 builder.Services.AddAutoMapper(typeof(Program).Assembly);
 //Authentication now!!!;
 builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(x=>{
     x.TokenValidationParameters = new TokenValidationParameters{
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value!)),
      ValidateIssuer = false,
      ValidateAudience = false

     };
 });
 builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Register for authentication;
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
