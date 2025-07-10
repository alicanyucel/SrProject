

using MediatR;


public record SendMailCommand(string ToEmail, string Subject, string Body) : IRequest<bool>;
