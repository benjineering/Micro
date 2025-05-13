//using Micro.IoC;

var builder = WebApplication.CreateSlimBuilder(args);
//builder.Services.ConfigureMicroJsonContexts();

var app = builder.Build();
//app.MapMicroEndpoints();
app.Run();
