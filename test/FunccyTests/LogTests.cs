using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Funccy;

namespace FunccyTests
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void Log_Test()
        {
            var _logger = new TestLogger();

            string logValue(int x) => $"value is {x}";

            var (result, logs) =  123
                .AsLog<int, string>()
                .Map(x => x + 543)
                .Add(logValue)
                .Map(x => x + 333)
                .Add(logValue)
                .Extract()
                ;

            logs.ForEach(_logger.Info);

            Assert.AreEqual(999, result);
            Assert.AreEqual("value is 666, value is 999", _logger.Log);
        }
    }

    public class TestLogger
    {
        public List<string> _log = new List<string>();

        public void Info(string message)
        {
            _log.Add(message);
        }

        public string Log => string.Join(", ", _log);
    }
}
