
namespace Helpers
{ 
    public interface IInjectorUser 
    {
        public void ConfigureInjector(DependencyInjector inj);
        public void GetDependencies();
    }
}