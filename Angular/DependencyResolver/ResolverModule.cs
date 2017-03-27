using System.Data.Entity;
using BLL.Interfaces.Services;
using BLL.Services;
using DAL.Concrete;
using DAL.Interfaces.Repository;
using Ninject;
using Ninject.Web.Common;
using ORM;

namespace DependencyResolver
{
    public static class ResolverModule
    {
        public static void ConfigurateResolverWeb(this IKernel kernel)
        {
            Configure(kernel, true);
        }

        public static void ConfigurateResolverConsole(this IKernel kernel)
        {
            Configure(kernel, false);
        }

        public static void Configure(IKernel kernel, bool isWeb)
        {
            if (isWeb)
            {
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
                kernel.Bind<DbContext>().To<interiordesigndbContext>().InRequestScope();
            }
            else
            {
                kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InSingletonScope();
                kernel.Bind<DbContext>().To<interiordesigndbContext>().InSingletonScope();
            }

            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IExtensionService>().To<ExtensionService>();
            kernel.Bind<IAlbumService>().To<AlbumService>();
            kernel.Bind<IImageService>().To<ImageService>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<IExtensionRepository>().To<ExtensionRepository>();
            kernel.Bind<IAlbumRepository>().To<AlbumRepository>();
            kernel.Bind<IImageRepository>().To<ImageRepository>();
        }
    }
}
