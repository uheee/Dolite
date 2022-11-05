namespace Dolite.Utils;

public class ErrorInfo
{
    public int ErrCode { get; init; } = -1;
    public string ErrMsg { get; init; } = null!;
    public IEnumerable<string>? Stacktrace { get; init; }
    public ErrorInfo? Inner { get; init; }
}