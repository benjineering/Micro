var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    foreach (var (context, i) in Micro.Generated.SerializerContexts.Select((x, i) => (x, i)))
    {
        options.SerializerOptions.TypeInfoResolverChain.Insert(i, context);
    }
});

var app = builder.Build();

foreach (var endpointGroup in Micro.Generated.EndpointGroups)
{
    var group = app.MapGroup($"/{endpointGroup.Name}");

    foreach (var endpoint in group.Endpoints)
    {
        group.MapGet($"/{endpoint.Name}", () => endpoint.Method);
    }
}

app.Run();
