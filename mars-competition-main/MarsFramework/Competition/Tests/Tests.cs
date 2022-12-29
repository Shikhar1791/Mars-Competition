using Competition.Pages;
using NUnit.Framework;
using static Competition.Global.GlobalDefinitions;
using static Competition.Pages.ShareSkill;

namespace Competition
{
    [TestFixture]
    [Parallelizable]
    internal class Tests : Global.Base

    {
        ManageListings manageListingObj;
        ShareSkill shareSkillObj;

        public Tests()
        {
            manageListingObj = new ManageListings();
            shareSkillObj = new ShareSkill();

        }

        [Test, Order(1)]
        public void TC1a_WhenIEnterListing()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            manageListingObj.AddListing(2, "ManageListings");

        }
        [Test, Order(2)]
        public void TC1b_ThenListingIsCreated()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            VerifyListingDetails(2, "ManageListings");
        }

        [Test , Order(3)]

        public void TC2a_WhenIEditListing()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            manageListingObj.EditListing(2, 3, "ManageListings");
            

        }

        [Test , Order (4)]
        public void TC2b_ThenListingIsEdited()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            VerifyListingDetails(3, "ManageListings");
        }

        [Test , Order (5)]
        public void TC3a_WhenIDeleteListing()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            manageListingObj.DeleteListing(3, "ManageListings");
        }

        [Test, Order(6)]
        public void TC3b_ThenListingIsDeleted()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            VerifyDelete(3, "ManageListing");
        }

        #region Assertions for EnterShareSkills
        public void VerifyListingDetails(int rowNumber, string worksheet)
        {
            
            manageListingObj.ViewListing(rowNumber, worksheet);

            Listing excel = new Listing();
            Listing web = new Listing();

            shareSkillObj.GetExcel(rowNumber, worksheet, out excel);

            shareSkillObj.GetWeb(out web);
            //Assertion
            Assert.Multiple(() =>
            {
                //Verify expected title vs actual title
                Assert.AreEqual(excel.title, web.title);

                //Verify expected Description vs actual Description
                Assert.AreEqual(excel.description, web.description);

                //Verify expected category vs actual category
                Assert.AreEqual(excel.category, web.category);

                //Verify expected Sub-category vs actual Sub-Category
                Assert.AreEqual(excel.subcategory, web.subcategory);

                //Verify expected ServiceType vs actual Servicetype
                string serviceTypeText = "Hourly";
                if (excel.serviceType == "One-off service")
                    serviceTypeText = "One-off";
                Assert.AreEqual(serviceTypeText, web.serviceType);

                //Verify expected StartDate vs Actual StartDate
                string expectedStartDate = DateTime.Parse(excel.startDate).ToString("yyyy-MM-dd");
                Assert.AreEqual(expectedStartDate, web.startDate);

                //Verify expected EndDate vs Actual EndDate
                string expectedEndDate = DateTime.Parse(excel.endDate).ToString("yyyy-MM-dd");
                Assert.AreEqual(expectedEndDate, web.endDate);

                //Verify expected Location Type vs actual Location Type
                string expectedLocationType = excel.locationType;
                if (expectedLocationType.Equals("On-site"))
                    expectedLocationType = "On-site";
                Assert.AreEqual(expectedLocationType, web.locationType);

                //Verify Skills Trade
                if (excel.skillTrade == "Credit")
                    Assert.AreEqual("None Specified", shareSkillObj.GetSkillTrade("Credit"));
                else
                    Assert.AreEqual(excel.skillExchange, shareSkillObj.GetSkillTrade("Skill-exchange"));
            });
            

        }

        public void VerifyDelete(int rowNumber, string worksheet)
        {
            //Poppulate excel data
            ExcelLib.PopulateInCollection(ExcelPath, worksheet);
            string title = ExcelLib.ReadData(rowNumber, "Title");
            //Click on Manage Listing
            manageListingObj.GotoManageListings();
            //Assertion
            Assert.AreEqual(title, manageListingObj.FindTitle("Title"), "Delete Failed");
        }
        #endregion
    }
}
