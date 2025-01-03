using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using MiniApi8.Models;
namespace MiniApi8.Endpoints;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Student").WithTags(nameof(Student));

        group.MapGet("/", () =>
        {
            return new [] { new Student() };
        })
        .WithName("GetAllStudents")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            return new Student { Id = id };
        })
        .WithName("GetStudentById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Student input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateStudent")
        .WithOpenApi();

        group.MapPost("/", (Student model) =>
        {
            return TypedResults.Created($"/api/Students/{model.Id}", model);
        })
        .WithName("CreateStudent")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            return TypedResults.Ok(new Student { Id = id });
        })
        .WithName("DeleteStudent")
        .WithOpenApi();
    }
}
