using { SolutionName }.Domain.Abstractions;

namespace { SolutionName }.Domain.Errors;

public static class Errors
{
    public static Error InvalidAction<T>(string message, T data) => new(message, data);

    public static Error Exception(Exception ex) => new(ex.Message, ex.Data);

    public static Error HandleException(Exception ex) => new(ex.Message, ex);

    public static readonly string ErrorMsg = "ErrorMsg";
}
