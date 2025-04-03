using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace edemBack
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Логируем входящий запрос
            context.Request.EnableBuffering(); // Позволяет читать тело запроса несколько раз
            var requestBody = await ReadRequestBody(context.Request);
            var queryString = GetQueryString(context.Request);
            _logger.LogInformation("➡ [Запрос] {Method}{Path}{QueryString} \nТело: {RequestBody}",
                context.Request.Method, context.Request.Path, queryString, requestBody);

            // Перехватываем ответ
            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context); // Передаём управление следующему middleware

            stopwatch.Stop();
            var responseBody = await ReadResponseBody(context.Response);
            _logger.LogInformation("⬅ [Ответ] {Method} {Path}{QueryString} \nСтатус: {StatusCode} ({ElapsedMilliseconds} ms) \nТело: {ResponseBody}",
                context.Request.Method, context.Request.Path, queryString, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, responseBody);

            await responseBodyStream.CopyToAsync(originalBodyStream); // Возвращаем тело ответа
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            if (request.Method == HttpMethods.Get) return "GET-запрос без тела";

            request.Body.Position = 0;
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Position = 0; // Восстанавливаем позицию
            return string.IsNullOrWhiteSpace(body) ? "Пустое тело" : body;
        }

        private string GetQueryString(HttpRequest request)
        {
            return request.QueryString.HasValue ? request.QueryString.Value : "Нет параметров";
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(response.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return string.IsNullOrWhiteSpace(body) ? "Пустой ответ" : body;
        }
    }
}
