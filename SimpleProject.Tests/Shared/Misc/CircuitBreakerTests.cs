using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleProject.Shared.Misc;
using System.Threading.Tasks;

namespace SimpleProject.Tests.Shared.Misc
{
    [TestClass]
    public class CircuitBreakerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var circuitBreaker = new CircuitBreaker();

            for (var i = 0; i < 20; i++)
            {
                try
                {
                    var result = circuitBreaker.Execute(() =>
                    {
                        ChaosMonkey.Do();

                        return Task.FromResult(0);
                    }).GetAwaiter().GetResult();
                }
                catch { }
            }
        }
    }
}