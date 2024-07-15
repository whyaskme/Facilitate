using Facilitate.Libraries.Models;
using Facilitate.Libraries.Services;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using IPNetwork = Microsoft.AspNetCore.HttpOverrides.IPNetwork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get forwarded headers from the load balancer if available
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // These three subnets encapsulate the applicable Azure subnets. At the moment, it's not possible to narrow it down further.
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:10.0.0.0"), 104));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:192.168.0.0"), 112));
    options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("::ffff:172.16.0.0"), 108));
});

// Configure the database service
builder.Services.Configure<DBSettings>(builder.Configuration.GetSection("FacilitateDatabase"));
builder.Services.AddSingleton<DBService>();

builder.Services.AddSingleton<Utils>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthorization();

// Use forwarded headers by the load balancer when available
app.UseForwardedHeaders();

// Redirect to https
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
