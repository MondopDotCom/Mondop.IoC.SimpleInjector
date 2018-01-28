using Mondop.Abstractions.IoC;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace Mondop.IoC.SimpleInjector
{
    public class Bootstrapper
    {
        private readonly Dictionary<Type, IIoCModule> _modules = new Dictionary<Type, IIoCModule>();
        private readonly List<Type> _moduleTypes = new List<Type>();
        public readonly Container _container;
        private readonly SimpleInjectionIoCContainer _iocContainer;

        public Bootstrapper()
        {
            _container = new Container();
            _iocContainer = new SimpleInjectionIoCContainer(_container);
        }

        private void RegisterDependencies(IIoCModule module)
        {
            foreach (var dependency in module.DependsOn)
                RegisterModule(dependency);
        }

        private void RegisterModule(Type moduleType)
        {
            if(!_modules.ContainsKey(moduleType))
            {
                var instance = Activator.CreateInstance(moduleType) as IIoCModule;
                instance.Register(_iocContainer);
                _modules.Add(moduleType, instance);

                RegisterDependencies(instance);
            }
        }

        private void RegisterModules()
        {
            foreach (var moduleType in _moduleTypes)
                RegisterModule(moduleType);
        }

        public void AddModule(Type moduleType)
        {
            _moduleTypes.Add(moduleType);
        }

        protected virtual void BeforeBootup()
        {

        }

        protected virtual void AfterBootup()
        {

        }

        public void Bootup()
        {
            BeforeBootup();
            RegisterModules();
            AfterBootup();
        }

        public TService GetInstance<TService>() where TService: class
        {
            return (TService)_container.GetInstance<TService>();
        }
    }
}
