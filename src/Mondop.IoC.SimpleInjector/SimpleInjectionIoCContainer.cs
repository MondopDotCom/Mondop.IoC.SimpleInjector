using Mondop.Abstractions.IoC;
using Mondop.Core;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace Mondop.IoC.SimpleInjector
{
    public class SimpleInjectionIoCContainer : IIoCContainer
    {
        private readonly Container _container;

        public SimpleInjectionIoCContainer(Container container)
        {
            _container = Ensure.IsNotNull(container,nameof(container));
        }

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.Register<TService, TImplementation>();
        }

        public void Register<TService>() where TService : class
        {
            _container.Register<TService>();
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            _container.RegisterSingleton<TService, TImplementation>();
        }

        public void RegisterCollection<TService>(IEnumerable<Type> implementations) where TService : class
        {
            _container.RegisterCollection<TService>(implementations);
        }
    }
}
