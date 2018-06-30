using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mondop.IoC.SimpleInjector.Tests.TestClasses;
using System;

namespace Mondop.IoC.SimpleInjector.Tests
{
    [TestClass]
    public class SimpleInjectionIoCContainerTests
    {
        private SimpleInjectionIoCContainer _simpleInjectionIoCContainer;

        [TestInitialize]
        public void TestInitialize()
        {
            _simpleInjectionIoCContainer = new SimpleInjectionIoCContainer();
        }

        [TestMethod]
        public void CallRegister_ServiceShould_Be_Registered()
        {
            _simpleInjectionIoCContainer.Register<ITestService, TestService1>();
            _simpleInjectionIoCContainer.Build();

            var implementation = _simpleInjectionIoCContainer.GetInstance<ITestService>();

            implementation.Should().NotBeNull();
            implementation.Should().BeOfType<TestService1>();
        }

        [TestMethod]
        public void CallRegisterTwice_Expect_InvalidOperationException()
        {
            _simpleInjectionIoCContainer.Register<ITestService, TestService1>();

            Action action = () => _simpleInjectionIoCContainer.Register<ITestService, TestService2>();

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [TestMethod]
        public void CallRegister_WithoutInterface_ServiceShould_Be_Registered()
        {
            _simpleInjectionIoCContainer.Register<TestService1>();
            _simpleInjectionIoCContainer.Build();

            var implementation = _simpleInjectionIoCContainer.GetInstance<TestService1>();

            implementation.Should().NotBeNull();
            implementation.Should().BeOfType<TestService1>();
        }

        [TestMethod]
        public void CallRegisterSingleton_ServiceShould_Be_Registered()
        {
            _simpleInjectionIoCContainer.RegisterSingleton<ITestService, TestService1>();
            _simpleInjectionIoCContainer.Build();

            var implementation = _simpleInjectionIoCContainer.GetInstance<ITestService>();

            implementation.Should().NotBeNull();
            implementation.Should().BeOfType<TestService1>();
        }

        [TestMethod]
        public void CallRegisterSingletonTwice_Expect_InvalidOperationException()
        {
            _simpleInjectionIoCContainer.RegisterSingleton<ITestService, TestService1>();

            Action action = () => _simpleInjectionIoCContainer.RegisterSingleton<ITestService, TestService2>();

            action.Should().ThrowExactly<InvalidOperationException>();
        }

        [TestMethod]
        public void CallRegisterCollection_ServiceShould_Be_Registered()
        {
            _simpleInjectionIoCContainer.RegisterCollection<ITestService>(
                new[] { typeof(TestService1), typeof(TestService2) });

            _simpleInjectionIoCContainer.RegisterCollection<ITestService>(
                new[] { typeof(TestService3) });

            _simpleInjectionIoCContainer.Build();

            var implementation = _simpleInjectionIoCContainer.GetAllInstances<ITestService>();
            
            implementation.Should().NotBeNull();
            implementation.Should().HaveCount(3);
        }
    }
}
