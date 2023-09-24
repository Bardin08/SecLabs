using FastEndpoints;
using SecurityLabs;

var builder = WebApplication
    .CreateBuilder(args);

builder.AddPresentationLayerServices();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();
