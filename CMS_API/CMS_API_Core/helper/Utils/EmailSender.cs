using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CMS_API_Core.helper.Response;
using Microsoft.AspNetCore.Http;

namespace CMS_API_Core.helper.Utils
{
    public class EmailSender : ControllerBase
    {


        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {


            _configuration = configuration;
        }

        public ApiResponse<object?> EmailMessage(string to, string subject, string content)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_configuration.GetValue<String>("EmailService:Email"));
                message.Subject = subject;
                message.To.Add(new MailAddress(to));
                message.Body = content;
                message.IsBodyHtml = true;

                var SmtpClient = new SmtpClient(_configuration.GetValue<String>("EmailService:Host"), _configuration.GetValue<int>("EmailService:Port"))
                {
                    Credentials = new NetworkCredential(_configuration.GetValue<String>("EmailService:Email"), _configuration.GetValue<String>("EmailService:Password")),
                    EnableSsl = true
                };

                SmtpClient.Send(message);



                return new ApiResponse<object?>(
                             data: null,
                             status: StatusCodes.Status200OK,
                             message: ["the email send Successfuly"]);

            }
            catch (System.Exception e)
            {

                return new ApiResponse<object?>(
               data: null, // Error message
               status: StatusCodes.Status400BadRequest,
               message: [" the email couldn't be send please try again" + e]);

            }

        }

    }
}
