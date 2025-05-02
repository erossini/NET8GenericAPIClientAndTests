using PSC.CSharp.Library.Extensions;

namespace PSC.CSharp.Library.APIClient.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        [TestMethod()]
        [TestCategory("Extensions")]
        [DataRow("/", "/", null)]
        [DataRow("/test", "/test", null)]
        [DataRow("/t1?q=1", "/t1", "q=1")]
        [DataRow("/t1?q1=1&q2=2", "/t1", "q1=1&q2=2")]
        public void BuildUriTest(string expected, string baseUrl, string? query = null)
        {
            var rsl = baseUrl.BuildUri(query);
            Assert.AreEqual(expected, rsl);
        }

        [TestMethod()]
        [TestCategory("Extensions")]
        [DataRow("/test", "/", "test/", null)]
        [DataRow("/t1/t2", "/t1", "/t2/", null)]
        [DataRow("/t1/t2?q=1", "/t1", "t2", "q=1")]
        [DataRow("/t1/t2?q=1", "/t1", "/t2/", "q=1")]
        [DataRow("/t1/t2?q1=1&q2=2", "/t1", "/t2/", "q1=1&q2=2")]
        public void BuildUriTest_OK(string expected, string baseUrl, string? path = null, string? query = null)
        {
            var rsl = baseUrl.BuildUri(path, query);
            Assert.AreEqual(expected, rsl);
        }

        [TestMethod()]
        [TestCategory("Extensions")]
        [DataRow("/", "//")]
        [DataRow("/test/test1", "//test//test1")]
        public void FormatUrlTest(string expected, string value)
        {
            var rsl = value.FormatUrl();
            Assert.AreEqual(expected, rsl);
        }
    }
}
