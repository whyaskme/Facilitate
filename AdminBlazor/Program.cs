using AdminBlazor.Data;
using AdminBlazor.Components;

using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});

builder.Services.AddSingleton<WebServices>();

builder.Services.AddCors(x => x.AddPolicy("externalRequests",
                    policy => policy
                .WithOrigins("https://jsonip.com")));

// At the ConfigureServices section in Startup.cs for MongoDB
builder.Services
    .AddIdentityCore<MongoUser>()
    .AddRoles<MongoRole>()
    .AddMongoDbStores<MongoUser, MongoRole, ObjectId>(mongo =>
    {
        mongo.ConnectionString = "mongodb://127.0.0.1:27017/Facilitate";
        // other options
    })
    .AddDefaultTokenProviders();

builder.Services.AddIdentityMongoDbProvider<MongoUser, MongoRole>(identity =>
{
    identity.Password.RequiredLength = 8;
    // other options
},
    mongo =>
    {
        mongo.ConnectionString = "mongodb://127.0.0.1:27017/Facilitate";
        // other options
    });

var app = builder.Build();

app.UseCors("externalRequests");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();