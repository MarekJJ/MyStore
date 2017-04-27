using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using MyStoreDomain.Abstract;
using MyStoreDomain.Concrete;
using MyStore.Domain.Concrete;
using MyStoreWebUI.Infastructure.Abstract;
using MyStoreWebUI.Infastructure.Concrete;

namespace MyStoreWebUI.Infastructure                     // ninject asosiation interface with classes
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {    //Moq list which you can check if everything work before conected database
            //            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            //            mock.Setup(m => m.Myitems).Returns(new List<MyItem> {
            //new MyItem { Name1 = "Piłka nożna", Price = 25 },
            //new MyItem { Name1 = "Deska surfingowa", Price = 179 },
            //new MyItem { Name1 = "Buty do biegania", Price = 95 }
            //});
            //            kernel.Bind<IitemsRepository>().ToConstant(mock.Object);


            kernel.Bind<IitemsRepository>().To<EFItemsRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}