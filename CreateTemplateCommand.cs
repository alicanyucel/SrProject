public record CreateTemplateCommand : IRequest<Result<string>>, IBaseRequest, IEquatable<CreateTemplateCommand>
{
    public string Name { get; init; }
    public string RaporTipi { get; init; }
    public string ContextHtml { get; init; }
    public string Content { get; init; } // Added missing property
}
