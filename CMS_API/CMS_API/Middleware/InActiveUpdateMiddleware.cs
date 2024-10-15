using CMS_API_Infrastructure.DBcontext;
using System.Security.Claims;

namespace CMS_API.Middleware
{
    public class InActiveUpdateMiddleware
    {
        private readonly RequestDelegate _next;

        public InActiveUpdateMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext dbContext)
        {
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                // Check if the user is authenticated
                var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    // Retrieve the session and update inactive date before calling the next middleware
                    var session = await dbContext.Session.FindAsync(userId);
                    if (session != null)
                    {
                        session.InActiveDate = DateTime.Now;
                        dbContext.SaveChanges();
                    }
                }
            }
            // Now continue with the next middleware

        }
    }
}
