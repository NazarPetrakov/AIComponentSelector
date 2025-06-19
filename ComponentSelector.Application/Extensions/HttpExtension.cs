using System;
using System.Text.Json;
using ComponentSelector.Application.Helpers.Pagination;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.Extensions;

public static class HttpExtension
{
    public static void AddPaginationHeader<T>(this HttpResponse response, PagedList<T> data)
    {
        var metaData = new PaginationMetadata(data.CurrentPage,
            data.PageSize, data.TotalCount, data.TotalPages, data.HasPreviousPage, data.HasNextPage);

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var jsonData = JsonSerializer.Serialize(metaData, jsonOptions);

        response.Headers.Append("Pagination", jsonData);
        response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
    }

}
