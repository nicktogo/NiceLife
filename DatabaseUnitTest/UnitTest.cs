using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

using NiceLife.Weather.Database;
using SQLitePCL;
using System;
using System.Collections.Generic;

namespace DatabaseUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public static string weatherInfoResponse = @"<resp>
< city > 嘉定 </ city >
< updatetime > 19:29</updatetime>
<wendu>7</wendu>
<fengli>1级</fengli>
<shidu>81%</shidu>
<fengxiang>西风</fengxiang>
<sunrise_1>06:52</sunrise_1>
<sunset_1>16:58</sunset_1>
<sunrise_2/>
<sunset_2/>
<alarm>
<cityKey>10102</cityKey>
<cityName>
<![CDATA[上海市]]>
</cityName>
<alarmType>
<![CDATA[霾]]>
</alarmType>
<alarmDegree>
<![CDATA[黄色]]>
</alarmDegree>
<alarmText>
<![CDATA[上海市气象台发布霾黄色预警]]>
</alarmText>
<alarm_details>
<![CDATA[
奉贤气象台2015年12月25日05时55分发布霾黄色预警信号: 目前本区已出现中度霾，预计将持续到明天白天, 请注意防范。
]]>
</alarm_details>
<standard/>
<suggest/>
<imgUrl>
<![CDATA[
http://static.etouch.cn/apps/weather/alarm_icon-1/mai_yellow-1.png
]]>
</imgUrl>
<time>2015-12-25 05:55:00</time>
</alarm>
<yesterday>
<date_1>24日星期四</date_1>
<high_1>高温 12℃</high_1>
<low_1>低温 5℃</low_1>
<day_1>
<type_1>小雨</type_1>
<fx_1>东北风</fx_1>
<fl_1>微风</fl_1>
</day_1>
<night_1>
<type_1>阴</type_1>
<fx_1>东北风</fx_1>
<fl_1>微风</fl_1>
</night_1>
</yesterday>
<forecast>
<weather>
<date>25日星期五</date>
<high>高温 10℃</high>
<low>低温 3℃</low>
<day>
<type>多云</type>
<fengxiang>西北风</fengxiang>
<fengli>微风级</fengli>
</day>
<night>
<type>多云</type>
<fengxiang>西风</fengxiang>
<fengli>微风级</fengli>
</night>
</weather>
<weather>
<date>26日星期六</date>
<high>高温 14℃</high>
<low>低温 5℃</low>
<day>
<type>多云</type>
<fengxiang>西南风</fengxiang>
<fengli>微风级</fengli>
</day>
<night>
<type>多云</type>
<fengxiang>北风</fengxiang>
<fengli>微风级</fengli>
</night>
</weather>
<weather>
<date>27日星期天</date>
<high>高温 11℃</high>
<low>低温 5℃</low>
<day>
<type>阴</type>
<fengxiang>东北风</fengxiang>
<fengli>微风级</fengli>
</day>
<night>
<type>多云</type>
<fengxiang>东北风</fengxiang>
<fengli>微风级</fengli>
</night>
</weather>
<weather>
<date>28日星期一</date>
<high>高温 10℃</high>
<low>低温 3℃</low>
<day>
<type>多云</type>
<fengxiang>东北风</fengxiang>
<fengli>微风级</fengli>
</day>
<night>
<type>多云</type>
<fengxiang>东风</fengxiang>
<fengli>微风级</fengli>
</night>
</weather>
<weather>
<date>29日星期二</date>
<high>高温 10℃</high>
<low>低温 4℃</low>
<day>
<type>多云</type>
<fengxiang>东风</fengxiang>
<fengli>微风级</fengli>
</day>
<night>
<type>多云</type>
<fengxiang>东北风</fengxiang>
<fengli>微风级</fengli>
</night>
</weather>
</forecast>
<zhishus>
<zhishu>
<name>晨练指数</name>
<value>不宜</value>
<detail>早晨天气寒冷，请尽量避免户外晨练，若坚持室外晨练请注意保暖防冻，建议年老体弱人群适当减少晨练时间。</detail>
</zhishu>
<zhishu>
<name>舒适度</name>
<value>舒适</value>
<detail>白天不太热也不太冷，风力不大，相信您在这样的天气条件下，应会感到比较清爽和舒适。</detail>
</zhishu>
<zhishu>
<name>穿衣指数</name>
<value>较冷</value>
<detail>建议着厚外套加毛衣等服装。年老体弱者宜着大衣、呢外套加羊毛衫。</detail>
</zhishu>
<zhishu>
<name>感冒指数</name>
<value>较易发</value>
<detail>天凉，昼夜温差较大，较易发生感冒，请适当增减衣服，体质较弱的朋友请注意适当防护。</detail>
</zhishu>
<zhishu>
<name>晾晒指数</name>
<value>基本适宜</value>
<detail>天气不错，午后温暖的阳光仍能满足你驱潮消霉杀菌的晾晒需求。</detail>
</zhishu>
<zhishu>
<name>旅游指数</name>
<value>适宜</value>
<detail>天气较好，但丝毫不会影响您出行的心情。温度适宜又有微风相伴，适宜旅游。</detail>
</zhishu>
<zhishu>
<name>紫外线强度</name>
<value>最弱</value>
<detail>属弱紫外线辐射天气，无需特别防护。若长期在户外，建议涂擦SPF在8-12之间的防晒护肤品。</detail>
</zhishu>
<zhishu>
<name>洗车指数</name>
<value>较适宜</value>
<detail>较适宜洗车，未来一天无雨，风力较小，擦洗一新的汽车至少能保持一天。</detail>
</zhishu>
<zhishu>
<name>运动指数</name>
<value>较适宜</value>
<detail>天气较好，但考虑气温较低，推荐您进行室内运动，若户外适当增减衣物并注意防晒。</detail>
</zhishu>
<zhishu>
<name>约会指数</name>
<value>适宜</value>
<detail>天气较好，和恋人一起徜徉于熙攘人群中或漫步于柔软草地上，都是不错的主意哦。</detail>
</zhishu>
<zhishu>
<name>雨伞指数</name>
<value>不带伞</value>
<detail>天气较好，不会降水，因此您可放心出门，无须带雨伞。</detail>
</zhishu>
</zhishus>
</resp>
<!--  127.0.0.1(127.0.0.1):36887 ; 127.0.0.1:8080  -->";

        [TestMethod]
        public void Teststart()
        {
            CreateDatabase.LoadDatabase();
            TestProvinceHelper();
            TestCityHelper();
            TestCountyHelper();
            TestForecastHelper();
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

        public void TestForecastHelper()
        {
            List<Forecast> forecasts = new List<Forecast>();
            DateTime today = DateTime.Now;
            for (int i = 0; i < 5; i++)
            {
                Forecast f = new Forecast();
                f.date = today.AddDays(i);
                forecasts.Add(f);
            }
            ForecastHelper helper = ForecastHelper.GetHelper();
            helper.InsertItems(forecasts);

            List<Forecast> actual = helper.SelectGroupItems();
            Assert.AreEqual(forecasts.Count, actual.Count);
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
