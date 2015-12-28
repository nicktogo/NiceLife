using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using NiceLife.Weather.Database;
using SQLitePCL;
namespace DatabaseUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Teststart()
        {
            CreateDatabase.LoadDatabase();
            TestProvinceHelper();
            TestCityHelper();
            TestCountyHelper();
        }

        public void TestProvinceHelper()
        {
            Province province = new Province();
            province.Id = 1;
            province.Name = "上海";
            province.Code = "34562";

            ProvinceHelper helper = ProvinceHelper.GetHelper();

            helper.InsertSingleItem(province);
            Province ActualItem = helper.SelectSingleItemById(province.Id);
            AssertEqual(province, ActualItem);


        }

        public void TestCityHelper()
        {
            City city = new City();
            city.Id = 1;
            city.Name = "嘉定";
            city.Code = "3425";
            city.ProvinceId = 1;

            CityHelper helper = CityHelper.GetHelper();

            helper.InsertSingleItem(city);
            City ActualItem = helper.SelectSingleItemById(city.Id);
            AssertEqual(city, ActualItem);

        }

        public void TestCountyHelper()
        {
            County county = new County();
            county.Id = 1;
            county.Name = "嘉定";
            county.Code = "34345";
            county.CityId = 1;

            CountyHelper helper = CountyHelper.GetHelper();

            helper.InsertSingleItem(county);
            County actual = helper.SelectSingleItemById(county.Id);
            AssertEqual(county, actual);

            county.CountySelect = 1;
            helper.UpdateSingleItem(county);
            actual = helper.SelectSingleItemById(county.Id);
            AssertEqual(county, actual);

        }

        private void AssertEqual(Province expect, Province actual)
        {
            Assert.AreEqual(expect.Name, actual.Name);
            Assert.AreEqual(expect.Code, actual.Code);
        }

        private void AssertEqual(City expect, City actual)
        {
            Assert.AreEqual(expect.Name, actual.Name);
            Assert.AreEqual(expect.Code, actual.Code);
        }

        private void AssertEqual(County expect, County actual)
        {
            Assert.AreEqual(expect.Name, actual.Name);
            Assert.AreEqual(expect.Code, expect.Code);
            Assert.AreEqual(expect.CountySelect, actual.CountySelect);
        }
    }
}
