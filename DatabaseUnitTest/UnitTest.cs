using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using NiceLife.Weather.Database;
using SQLitePCL;
namespace DatabaseUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateDB()
        {
            SQLiteResult expect = SQLiteResult.DONE;
            SQLiteResult actual =  CreateDatabase.LoadDatabase();
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void TestProvinceHelper()
        {
            Province province = new Province();
            province.Name = "上海";
            province.Code = "34562";

            ProvinceHelper helper = ProvinceHelper.GetHelper();

            helper.InsertSingleItem(province);
            Province ActualItem = helper.SelectSingleItemById(province.Id);
            AssertEqual(province, ActualItem);


        }

        private void AssertEqual(Province expect, Province actual)
        {
            Assert.AreEqual(expect.Name, actual.Name);
            Assert.AreEqual(expect.Code, actual.Code);
        }
    }
}
