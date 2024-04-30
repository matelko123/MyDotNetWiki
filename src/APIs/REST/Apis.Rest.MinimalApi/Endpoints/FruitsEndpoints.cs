using Apis.Rest.MinimalApi.Endpoints.Internal;
using Microsoft.AspNetCore.Mvc;

namespace Apis.Rest.MinimalApi.Endpoints;

public class FruitsEndpoints : IEndpoints
{
    private static readonly List<Fruit> Fruits =
    [
        new("Banana"), new("Apple"), new("Pineapple")
    ];

    public static void DefineEndpoints(IEndpointRouteBuilder app)
    {
        var fruitsGroup = app
            .MapGroup("api/Fruits")
            .WithTags("Fruits");

        fruitsGroup.MapGet("", () =>
        {
            return Results.Ok(Fruits);
        })
            .WithName("GetAllFruits")
            .Produces<List<Fruit>>()
            .WithOpenApi(operation =>
            {
                operation.Summary = "Return all";
                operation.Description = "Return all fruits";
                return operation;
            });

        fruitsGroup.MapPost("", ([FromBody] string fruit) =>
        {
            Fruit newFruit = new(fruit);
            Fruits.Add(newFruit);
            return Results.CreatedAtRoute("GetFruitById", new { newFruit.Id }, newFruit);
        })
            .WithName("CreateNewFruit")
            .Accepts<string>(EndpointConstants.ContentType)
            .Produces<Fruit>(StatusCodes.Status201Created)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Add new";
                operation.Description = "Add new fruit";
                return operation;
            });

        fruitsGroup.MapGet("{id:guid}", ([FromRoute] Guid id) =>
        {
            Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);
            return searchedFruit is null
                ? Results.NotFound()
                : Results.Ok(searchedFruit);
        })
            .WithName("GetFruitById")
            .Produces<Fruit>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Return one";
                operation.Description = "Return specific fruit";
                return operation;
            });

        fruitsGroup.MapPut("{id:guid}", ([FromRoute] Guid id, [FromBody] string fruit) =>
        {
            Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);

            if (searchedFruit is null)
            {
                return Results.NotFound();
            }

            searchedFruit.Name = fruit;
            return Results.Ok();
        })
            .WithName("UpdateFruit")
            .Accepts<string>(EndpointConstants.ContentType)
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Update";
                operation.Description = "Update existing fruit";
                return operation;
            });

        fruitsGroup.MapDelete("{id:guid}", ([FromRoute] Guid id) =>
        {
            Fruit? searchedFruit = Fruits.SingleOrDefault(x => x.Id == id);

            if (searchedFruit is null)
            {
                return Results.NotFound();
            }

            Fruits.Remove(searchedFruit);
            return Results.NoContent();
        })
            .WithName("DeleteFruit")
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound)
            .WithOpenApi(operation =>
            {
                operation.Summary = "Delete";
                operation.Description = "Delete existing fruit";
                return operation;
            });
    }
}