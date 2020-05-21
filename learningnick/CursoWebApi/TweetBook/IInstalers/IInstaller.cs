using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TweetBook.IInstalers
{
    public interface IInstaller
    {
         void InstallServices(IServiceCollection services,IConfiguration configuration);
    }
}