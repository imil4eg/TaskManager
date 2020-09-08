using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace TaskManager.DAL.Tests
{
    [TestClass]
    public sealed class ProjectRepositoryTests
    {
        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            var mock = new Mock<ICloudTableFactory>();
            mock.Setup(a => a.Get)
        }
    }
}
