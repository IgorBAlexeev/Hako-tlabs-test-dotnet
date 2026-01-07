using Microsoft.AspNetCore.Diagnostics;

namespace TestApp.Server
{
  public class GlobalExceptionHandler : IExceptionHandler
  {
    private readonly ILogger<GlobalExceptionHandler> logger;
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
      this.logger = logger;
    }
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
      var exceptionMessage = exception.Message;
      logger.LogError(
          "Error Message: {exceptionMessage}, Time of occurrence {time}",
          exceptionMessage, DateTime.UtcNow);
      // Return false to continue with the default behavior
      // Return true to signal that this exception is handled
      return ValueTask.FromResult(false);
    }
  }
}