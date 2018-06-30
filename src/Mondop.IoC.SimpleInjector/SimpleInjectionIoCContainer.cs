using Mondop.Abstractions.IoC;
using SimpleInjector;
using System;
using System.Collections.Generic;

namespace Mondop.IoC.SimpleInjector
{
    public class SimpleInjectionIoCContainer : IIoCContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, Type> _singleTons = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, List<Type>> _collections = new Dictionary<Type, List<Type>>();

        private Container _container;

        public void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            if (IsAlreadyRegistered(typeof(TService)))
                throw new InvalidOperationException($"Type {typeof(TService)} is already registered.");

            _registrations.Add(typeof(TService), typeof(TImplementation));
        }

        public void Register<TService>() where TService : class
        {
            if (IsAlreadyRegistered(typeof(TService)))
                throw new InvalidOperationException($"Type {typeof(TService)} is already registered.");

            _registrations.Add(typeof(TService), null);
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            if (IsAlreadyRegistered(typeof(TService)))
                throw new InvalidOperationException($"Type {typeof(TService)} is already registered.");

            _singleTons.Add(typeof(TService), typeof(TImplementation));
        }

        public void RegisterCollection<TService>(IEnumerable<Type> implementations) where TService : class
        {
            if (IsAlreadyRegisteredExceptCollections(typeof(TService)))
                throw new InvalidOperationException($"Type {typeof(TService)} is already registered.");

            if (!_collections.ContainsKey(typeof(TService)))
                _collections.Add(typeof(TService), new List<Type>());

            _collections[typeof(TService)].AddRange(implementations);
        }

        public TService GetInstance<TService>() where TService: class
        {
            return _container.GetInstance<TService>();
        }

        public IEnumerable<TService> GetAllInstances<TService>() where TService : class
        {
            return _container.GetAllInstances<TService>();
        }

        public void Build()
        {
            _container = new Container();

            foreach (var registrationKvp in _registrations)
            {
                if (registrationKvp.Value != null)
                    _container.Register(registrationKvp.Key, registrationKvp.Value);
                else
                    _container.Register(registrationKvp.Key);
            }

            foreach (var registrationKvp in _singleTons)
            {
                if (registrationKvp.Value != null)
                    _container.RegisterSingleton(registrationKvp.Key, registrationKvp.Value);
                else
                    _container.RegisterSingleton(registrationKvp.Key);
            }

            foreach(var registrationKvp in _collections)
            {
                _container.RegisterCollection(registrationKvp.Key, registrationKvp.Value);
            }
        }

        private bool IsAlreadyRegistered(Type type) => _registrations.ContainsKey(type) || _singleTons.ContainsKey(type) || _collections.ContainsKey(type);
        private bool IsAlreadyRegisteredExceptCollections(Type type) => _registrations.ContainsKey(type) || _singleTons.ContainsKey(type);
    }
}
