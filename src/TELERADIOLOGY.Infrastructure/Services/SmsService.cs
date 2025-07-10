using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Microsoft.Extensions.Configuration;
using TELERADIOLOGY.Application.Services;

namespace TELERADIOLOGY.Infrastructure.Sms
{
    public class SmsService : ISmsSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromPhoneNumber;

        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"]
                          ?? throw new ArgumentNullException("Twilio:AccountSid");
            _authToken = configuration["Twilio:AuthToken"]
                          ?? throw new ArgumentNullException("Twilio:AuthToken");
            _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"]
                          ?? throw new ArgumentNullException("Twilio:FromPhoneNumber");

            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            var to = new PhoneNumber(toPhoneNumber);
            var from = new PhoneNumber(_fromPhoneNumber);

            await MessageResource.CreateAsync(
                body: message,
                from: from,
                to: to
            );
        }
    }
}
