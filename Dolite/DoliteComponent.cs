using Microsoft.AspNetCore.Builder;

namespace Dolite;

public abstract class DoliteComponent
{
    public virtual void BeforeBuild(WebApplicationBuilder builder)
    {
    }

    public virtual void AfterBuild(WebApplication app)
    {
    }
}