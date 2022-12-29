using Competition.Global;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Competition.Global.GlobalDefinitions;

namespace Competition.Pages
{
    internal class ManageListings
    {
        #region Manage listing's page objects
        //ShareSkill Button
        private IWebElement btnShareSkill => driver.FindElement(By.LinkText("Share Skill"));

        //Manage Listings
        private IWebElement manageListingsLink => driver.FindElement(By.XPath("//a[@href='/Home/ListingManagement']"));

        //Message warning no listing
        private IWebElement warningMessage => driver.FindElement(By.XPath("//h3[contains(text(),'You do not have any service listings!')]"));

        //Title
        private IList<IWebElement> Titles => driver.FindElements(By.XPath("//div[@id='listing-management-section']//tbody/tr/td[3]"));

        //View button
        private IWebElement view => driver.FindElement(By.XPath("(//i[@class='eye icon'])[1]"));

        //Edit button
        private IWebElement edit => driver.FindElement(By.XPath("(//i[@class='outline write icon'])[1]"));

        //Yes/No button
        private IList<IWebElement> clickActionsButton => driver.FindElements(By.XPath("//div[@class='actions']/button"));

        //Save button
        private IWebElement btnSave => driver.FindElement(By.XPath("//input[@value='Save']"));
        #endregion
        ShareSkill shareSkillObj;

        internal void AddListing(int rowNumber, string worksheet)
        {
            shareSkillObj = new ShareSkill();
            btnShareSkill.Click();
            wait(2);
            shareSkillObj.EnterShareSkill(rowNumber, worksheet);
            wait(3);

        }

        //Edit listing

        internal void EditListing(int rowNumber1, int rowNumber2, string worksheet)
        {
            shareSkillObj=new ShareSkill();
            //click on manageListing
            GotoManageListings();
            wait(2);
            //Populate the Excel Sheet
            ExcelLib.PopulateInCollection(Base.ExcelPath, worksheet);

            //Read data
            string expectedTitle = ExcelLib.ReadData(rowNumber1, "Title");

            //Click on Edit button
            string e_Edit = "//div[@id=\"listing-management-section\"]//tbody/tr[" + GetTitleIndex(expectedTitle) + "]/td[8]/div/button[2]";
            IWebElement btnEdit = driver.FindElement(By.XPath(e_Edit));
            btnEdit.Click();
            wait(2);

            shareSkillObj.ClearData();
            shareSkillObj.EnterShareSkill(rowNumber2 , worksheet);
            wait(3);


        }

        internal void ViewListing(int rowNumber, string worksheet)
        {
            //click on Manage Listing
            GotoManageListings();
            wait(2);
            //Read data
            ExcelLib.PopulateInCollection(Base.ExcelPath, worksheet);
            string expectedTitle = ExcelLib.ReadData(rowNumber, "Title");
            //Click on button View
            string e_View = "//div[@id=\"listing-management-section\"]//tbody/tr[" + GetTitleIndex(expectedTitle) + "]/td[8]/div/button[1]";
            IWebElement btnView = driver.FindElement(By.XPath(e_View));
            btnView.Click();
            wait(2);


        }
        internal void DeleteListing(int rowNumber, string worksheet)
        {
            //click on manage listing
            GotoManageListings();

            //Populate the Excel sheet
            ExcelLib.PopulateInCollection(Base.ExcelPath, worksheet);

            //read data
            string isDelete = ExcelLib.ReadData(rowNumber, "isDelete");
            string expectedTitle = ExcelLib.ReadData(rowNumber, "Title");
            //Click on delete button
            string strDelete = "//div[@id=\"listing-management-section\"]//tbody/tr[" + GetTitleIndex(expectedTitle) + "]/td[8]/div/button[3]";
            IWebElement btnDelete = driver.FindElement(By.XPath(strDelete));
            btnDelete.Click();

            //Click Yes
            if(isDelete.Equals("Yes"))
            {
                clickActionsButton[1].Click();

            }
            else
            {
                //Click No
                clickActionsButton[0].Click();
            }
            Thread.Sleep(1000);

        }
        //Verify delete
        internal string FindTitle(string title)
        {
            //Verify if there is no listing
            string actualTitle = "null";
            int titleCount = Titles.Count();
            if (titleCount.Equals(0))
            {
                return actualTitle;
            }
            else
            {
                //Verify if title is deleted
                for (int i = 0; i < titleCount; i++)
                {
                    actualTitle = Titles[i].Text;
                    if (title.Equals(actualTitle))
                        break;
                }
                return actualTitle;
            }
        }

        

        //Function for Invalid data
        internal void EnterShareSkill_InvalidData(int testData, string worksheet)
        {
            shareSkillObj = new ShareSkill();
            //click on button ShareSkill
            btnShareSkill.Click();
            wait(1);

            //nter Invalid data
            shareSkillObj.EnterShareSkill_InvalidData(testData, "NegativeTc");
            Thread.Sleep(2000);

        }


        internal void GotoManageListings()
        {
            try
            {
                //Click on Manage listing
                manageListingsLink.Click();
            }
            catch (Exception ex)
            {
                Assert.Fail("Manage listing link is not found", ex.Message);
            }
            
        }

        //function to check title is existing and return title's positioning in manage listing
        internal string GetTitleIndex(string expectedTitle)
        {
            //Check if there is no listing's title
            string recordIndex = "";
            int titleCount = Titles.Count;
            if(titleCount.Equals(0))
            {
                Assert.Ignore("There is no listing record");
            }

            else
            {
                //Find title:Break loop when finding a title.Output: recordIndex
                for(int i = 0; i < titleCount; i++)
                {
                    string actualTitle = Titles[i].Text;
                    if(actualTitle.Equals(expectedTitle))
                    {
                        recordIndex = (i+1).ToString();
                        break;
                    }
                }
                //If title to delete is not found
                if(recordIndex.Equals(""))
                {
                    Assert.Ignore("Listing" + expectedTitle + "is not found");
                }
            }
            return recordIndex;

        }

    }
}
