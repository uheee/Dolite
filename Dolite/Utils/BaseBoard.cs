using AutoMapper;

namespace Dolite.Utils;

public abstract class BaseBoard
{
    public IMapper Mapper { get; init; } = null!;
    public Resource Resource { get; init; } = null!;
}