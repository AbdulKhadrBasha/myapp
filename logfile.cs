using System.Text;

namespace myapp
{
    public class AccessLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logFilePath;

        public AccessLogMiddleware(RequestDelegate next, string logFilePath)
        {
            _next = next;
            _logFilePath = logFilePath;
        }

        public async Task Invoke(HttpContext context)
        {
            string logMessage = $"{DateTime.Now} - {context.Connection.RemoteIpAddress} - {context.Request.Method} - {context.Request.Path}{context.Request.QueryString}\n";

            // Log the request
            await LogToFileAsync(logMessage);

            // Call the next middleware in the pipeline
            await _next(context);
        }

        private async Task LogToFileAsync(string message)
        {
            try
            {
                await File.AppendAllTextAsync(_logFilePath, message, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging to file: {ex.Message}");
                // You may want to handle this error more gracefully
            }
        }
    }
}
