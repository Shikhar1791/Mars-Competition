using Competition.Pages;
using NUnit.Framework;
using static Competition.Global.GlobalDefinitions;
using static Competition.Pages.ShareSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Competition.Tests
{
    [TestFixture]
    internal class Tests : Global.Base

    {
        ManageListings manageListingObj;
        ShareSkill shareSkillObj;

        [Test, Order(1)]
        public void TC1a_WhenIEnterListing()
        {
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            manageListingObj = new ManageListings();
            manageListingObj.AddListing(2, "ManageListings");
        }

    }
}
