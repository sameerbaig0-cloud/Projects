using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class RequestSizeMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestSizeMiddleware> _logger;

	public RequestSizeMiddleware(RequestDelegate next, ILogger<RequestSizeMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		context.Request.EnableBuffering(); // Allow the request body to be read multiple times

		if (context.Request.ContentLength > 1048576) // Adjust the limit as needed
		{
			_logger.LogInformation("Request size exceeded the limit.");
			context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
			await context.Response.WriteAsync("File size exceeds the allowed limit.");
			return;
		}

		await _next(context);
	}
}
