using System.Text.Json.Nodes;
using FormaTest;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, Context.Default);
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/{count}/{depth}/{value}",
    Results<Ok<JsonObject>, BadRequest<string>> (int count, int depth, string value) =>
    {
        if (JsonTree.ValidateRequest(count, depth, value) is string issue)
        {
            return TypedResults.BadRequest(issue);
        }
        return TypedResults.Ok(JsonTree.Build(count, depth, value));
    });

app.Run();

