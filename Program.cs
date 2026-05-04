var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<UserFlatService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<NoticeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
