using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using ArqEmCamadas.Domain.Handlers.PaginationHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace ArqEmCamadas.UnitTest.TestTools;

public static class UtilTools
{
    public static PageList<T> BuildPageList<T>(List<T> entityList) where T : class
    {
        const int pageSize = 10;
        const int pageNumber = 1;

        return new PageList<T>(entityList, entityList.Count, pageNumber, pageSize);
    }

    public static PageList<T> BuildPageList<T>(T entity) where T : class
    {
        const int pageSize = 10;
        const int pageNumber = 1;

        var entityList = new List<T>
        {
            entity
        };

        return new PageList<T>(entityList, entityList.Count, pageNumber, pageSize);
    }

    public static string GenerateStringByLength(int length)
    {
        const string chars = "test";

        var random = new Random();

        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static ClaimsPrincipal GetUserRole(string role) =>
        new(
            new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.Role, role)
            ]));

    public static string GetAuthorizedControllerRoles(MethodInfo methodInfo)
    {
        var authorizeAttribute =
            methodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), true).FirstOrDefault() as AuthorizeAttribute;

        return authorizeAttribute!.Roles!;
    }

    public static Func<IQueryable<T>, IIncludableQueryable<T, object>> BuildQueryableIncludeFunc<T>() where T : class =>
        It.IsAny<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();

    public static Expression<Func<T, bool>> BuildPredicateFunc<T>() where T : class =>
        It.IsAny<Expression<Func<T, bool>>>();

    public static Expression<Func<T, T>> BuildSelectorFunc<T>() where T : class =>
        It.IsAny<Expression<Func<T, T>>>();


    public static IFormFile BuildIFormFile(string extension = "pdf")
    {
        var bytes = "This is a dummy file"u8.ToArray();

        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", $"image.{extension}")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg",
            ContentDisposition = $"form-data; name=\"Image\"; filename=\"image.{extension}\""
        };
    }
}