using MailKit.Net.Smtp;
using MimeKit;
public class Mail
{
    public string MailAdress { get; set; }

    public MimeMessage buildMessage(string adress)
    {
        adress = MailAdress;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("", "")); ;
        message.To.Add(new MailboxAddress(" ", adress));
        message.Subject = "";
        BodyBuilder bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = "";

        message.Body = bodyBuilder.ToMessageBody();
        return message;
    }

    public SmtpClient connectorSmtp()
    {
        using (var client = new SmtpClient())
        {
            client.Connect("", 0, false);
            client.Authenticate("", "");
            // client.Disconnect(true);

            return client;
        }
    }




}