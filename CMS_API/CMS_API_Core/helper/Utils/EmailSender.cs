using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS_API_Core.helper.Utils
{
    public class EmailSender : ControllerBase
    {


        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {


            _configuration = configuration;
        }

        public ActionResult EmailMessage(string to, string subject, string content)
        {
            var Respons = new Dictionary<string, object>();
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

                Respons["Success"] = "the email send Successfuly";

                return Ok(Respons.ToList());


            }
            catch (System.Exception e)
            {
                Respons["Alert"] = " the email couldn't be send please try again" + e;

                return BadRequest(Respons.ToList());
            }

        }

    }
}
