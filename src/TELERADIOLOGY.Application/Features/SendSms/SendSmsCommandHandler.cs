using MediatR;
using TELERADIOLOGY.Application.Features.Sms;
using TELERADIOLOGY.Application.Services;
using TS.Result;

namespace TELERADIOLOGY.Application.Features.SendSms
{
    internal sealed class SendSmsCommandHandler : IRequestHandler<SendSmsCommand, Result<string>>
    {
        private readonly ISmsSender _smsSender;

        public SendSmsCommandHandler(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public async Task<Result<string>> Handle(SendSmsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _smsSender.SendSmsAsync(request.PhoneNumber, request.Message);
                return Result<string>.Succeed("SMS başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"SMS gönderilemedi: {ex.Message}");
            }
        }
    }
}

