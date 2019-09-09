namespace NSeed.Cli.Validation
{
    internal interface IValidator<TCommand>
    {
        ValidationResult Validate(TCommand command);
    }
}
