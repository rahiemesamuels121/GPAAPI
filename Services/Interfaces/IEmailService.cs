namespace GPACARICOMAPI.Services.Interfaces
{
    public interface IEmailService
    {
        public  Task sendEmail(String reciever, string subject, string body);
    }
}
