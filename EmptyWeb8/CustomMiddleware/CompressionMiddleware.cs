

using System.IO.Compression;
using System.Text;

namespace EmptyWeb8.CustomMiddleware;

public class CompressionMiddleware
{
    private readonly RequestDelegate _next;

    public CompressionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBody = context.Response.Body;

        using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        await _next(context);

        buffer.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(buffer).ReadToEndAsync();

        const int compressionThreshold = 1024;
        if (buffer.Length > compressionThreshold)
        {
            var compressedBytes = CompressResponse(responseBody);
            context.Response.ContentLength = compressedBytes.Length;
            context.Response.Body = originalBody;

            await context.Response.Body.WriteAsync(compressedBytes, 0, compressedBytes.Length);
        }
        else
        {
            buffer.Seek(0, SeekOrigin.Begin);
            await buffer.CopyToAsync(originalBody);
            context.Response.Body = originalBody;
        }
    }

    private byte[] CompressResponse(string responseBody)
    {
        var bytes = Encoding.UTF8.GetBytes(responseBody);
        using var outputStream = new MemoryStream();
        using var compressionStream = new GZipStream(outputStream, CompressionMode.Compress);
        compressionStream.Write(bytes, 0, bytes.Length);
        compressionStream.Close();
        return outputStream.ToArray();
    }
}

// using var memoryStream = new MemoryStream();
// using (var gzip = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Compress))
// using (var writer = new StreamWriter(gzip))
// {
//     writer.Write(text);
// }
// return memoryStream.ToArray();