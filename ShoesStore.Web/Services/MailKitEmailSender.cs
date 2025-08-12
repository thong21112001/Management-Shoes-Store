using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using IdentityApplicationUser = ShoesStore.Domain.Entities.ApplicationUser;

namespace ShoesStore.Web.Services
{
    public class MailKitEmailSender : IEmailSender<IdentityApplicationUser>
    {
        private readonly IConfiguration _cfg;
        public MailKitEmailSender(IConfiguration cfg) => _cfg = cfg;

        public Task SendConfirmationLinkAsync(IdentityApplicationUser user, string email, string link)
        => SendAsync(email, "Xác minh email", $"""Nhấp vào đây để xác minh: <a href="{link}">Xác minh</a>""");

        public Task SendPasswordResetLinkAsync(IdentityApplicationUser user, string email, string link)
            => SendAsync(email, "Đặt lại mật khẩu", $"""Đặt lại tại: <a href="{link}">Reset</a>""");

        public Task SendPasswordResetCodeAsync(IdentityApplicationUser user, string email, string code)
            => SendAsync(email, "Mã đặt lại", $"Mã của bạn: <b>{code}</b>");

        private async Task SendAsync(string to, string subject, string html)
        {
            var server = _cfg["MailSettings:Server"];
            var port = int.TryParse(_cfg["MailSettings:Port"], out var p) ? p : 587;
            var useStartTls = bool.TryParse(_cfg["MailSettings:UseStartTls"], out var t) ? t : true;
            var senderName = _cfg["MailSettings:SenderName"] ?? "Shoes Store";
            var senderMail = _cfg["MailSettings:SenderEmail"];
            var user = _cfg["MailSettings:UserName"];
            var pass = _cfg["MailSettings:Password"];

            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(senderName, senderMail));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart("html") { Text = html };

            using var client = new SmtpClient();
            var socket = useStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;
            await client.ConnectAsync(server, port, socket);
            if (!string.IsNullOrWhiteSpace(user))
                await client.AuthenticateAsync(user, pass);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
