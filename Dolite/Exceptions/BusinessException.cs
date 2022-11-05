namespace Dolite.Exceptions;

public class BusinessException : Exception
{
    public BusinessException(int errCode, string errMsg) : base($"{errCode}: {errMsg}")
    {
        ErrCode = errCode;
        ErrMsg = errMsg;
    }

    public int ErrCode { get; }
    public string ErrMsg { get; }
}