var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: MyAllowSpecificOrigins,
//               builder =>
//               {
//                   builder.WithOrigins(
//                       "http://localhost:3000",
//                       "http://www.contoso.com")
//                        .AllowAnyHeader()
//                        .AllowAnyMethod();
//               });
//});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
