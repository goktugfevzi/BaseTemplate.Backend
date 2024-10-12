using BaseTemplate.Business.Abstractions.Token;
using BaseTemplate.Business.Abstractions;
using BaseTemplate.Business.Services;
using BaseTemplate.Repository.Abstractions;
using BaseTemplate.Repository.Conrete;
using BaseTemplate.Shared.Services;
using System.Reflection;
using BaseTemplate.Repository.Contexts;
using BaseTemplate.Business.Services.Token;
using Autofac;

namespace BaseTemplate.Presentation.Modules
{
    public class RepoServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(GenericService<>)).As(typeof(IGenericService<>)).InstancePerLifetimeScope();

            var apiAssembly = Assembly.GetExecutingAssembly();
            var repoAssembly = Assembly.GetAssembly(typeof(ExampleContext));
            var businessAssembly = Assembly.GetAssembly(typeof(TokenHandler));
            var sharedAssembly = Assembly.GetAssembly(typeof(UserClaimService));

            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, businessAssembly, sharedAssembly).Where(x => x.Name.EndsWith("Repository")).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(apiAssembly, repoAssembly, businessAssembly, sharedAssembly).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<TokenHandler>().As<ITokenHandler>().InstancePerLifetimeScope();
        }
    }
}
