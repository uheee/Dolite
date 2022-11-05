using Dolite.Exceptions;

namespace Dolite.Utils;

public class ExceptionFactory : BaseBoard
{
    public BusinessException Business(int errCode, params object[] args)
    {
        var errTemplate = Resource["zh_cn"][$"errors:{errCode}"];
        var errMsg = string.Format(errTemplate, args);
        return new BusinessException(errCode, errMsg ?? "unknown");
    }
}