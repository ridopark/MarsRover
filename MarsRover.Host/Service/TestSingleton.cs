using MarsRover.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Host.Service
{
    public interface ITestSingleton : ISingletonDependency
    {
        void test();
    }

    public class TestSingleton : ITestSingleton
    {
        public void test()
        {
            Console.WriteLine("TEST");
        }
    }
}
