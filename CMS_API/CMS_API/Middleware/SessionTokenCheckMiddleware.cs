using CMS_API_Application.Dto;
using CMS_API_Core.helper.Response;
using CMS_API_Infrastructure.DBcontext;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace CMS_API.Middleware
{
    public class SessionTokenCheckMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context, DataContext dbContext)
        {
            Console.WriteLine("SessionTokenCheckMiddleware: InvokeAsync");
            // Check if the user is authenticated
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null && !context.Request.Path.ToString().Contains("Authentication"))
            {
                Console.WriteLine("userId: not null " + userId);
                // Retrieve the session and update inactive date before calling the next middleware
                var session = await dbContext.Session.Include(s => s.RefreshToken).FirstOrDefaultAsync((s) => s.UserId == userId);
                if (session != null)
                {
                    Console.WriteLine("session: not null " + session.IsExpired);
                    if (session.IsExpired)
                    {
                        Console.WriteLine("session: expired " + session.IsExpired);
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<object?>(
                                     data: null,
                                     status: StatusCodes.Status419AuthenticationTimeout,
                                     message: ["session experd"])));
                        return;
                    }

                    if (session.Token != context.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty))
                    {
                        Console.WriteLine("session: token not match ");
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status419AuthenticationTimeout,
                                message: ["Token experd"])));

                        return;
                    }


                }
                else
                {
                    Console.WriteLine("session: null " + session);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status500InternalServerError,
                           message: ["session deleted"])));
                    return;
                }
            }




            await next(context);
        }
    }
}
