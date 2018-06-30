using System;
using System.Collections.Generic;
using System.Text;

namespace Mondop.IoC.SimpleInjector.Tests.TestClasses
{
    public interface ITestService
    {
        string Name { get; }
    }

    public class TestService1: ITestService
    {
        public string Name => "TestService1";
    }

    public class TestService2 : ITestService
    {
        public string Name => "TestService2";
    }

    public class TestService3 : ITestService
    {
        public string Name => "TestService3";
    }
}
