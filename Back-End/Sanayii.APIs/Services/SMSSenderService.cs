using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Sanayii.APIs.Services
{
    public class SMSSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly string? AccountSID;
        private readonly string? AuthToken;
        private readonly string? FromNumber;
        public SMSSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
            AccountSID = _configuration["SMSSettings:AccountSID"];
            AuthToken = _configuration["SMSSettings:AuthToken"];
            FromNumber = _configuration["SMSSettings:FromNumber"];
        }
        public async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {

                if (string.IsNullOrEmpty(toPhoneNumber) || string.IsNullOrEmpty(message))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(AccountSID) || string.IsNullOrEmpty(AuthToken))
                {
                    throw new ArgumentException("Twilio Account SID and Auth Token must be provided in the configuration.");
                }
                if (!toPhoneNumber.StartsWith("+"))
                {
                    toPhoneNumber = "+" + toPhoneNumber;
                }
                TwilioClient.Init(AccountSID, AuthToken);
                var messageOptions = new CreateMessageOptions(new PhoneNumber(toPhoneNumber))
                {
                    From = new PhoneNumber(FromNumber),
                    Body = message
                };
                var msg = await MessageResource.CreateAsync(messageOptions);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}