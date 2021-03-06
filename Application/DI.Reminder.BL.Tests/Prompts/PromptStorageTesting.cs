﻿using Moq;
using DI.Reminder.BL.Cache;
using DI.Reminder.Data.CategoryDataBase;
using DI.Reminder.Data.PromptDataBase;
using DI.Reminder.Data.SearchingDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DI.Reminder.Common.PromptModel;
using DI.Reminder.Common.CategoryModel;
using System.Linq;
using DI.Reminder.BL.CategoryStorage;

namespace DI.Reminder.BL.Tests.Prompts
{
    [TestClass]
    public class PromptStorageTesting
    {
        private PromptStorage.Prompts _prompts;
        private Mock<ICacheService> _cacheRepository;
        private Mock<ISearchService> _search;
        private Mock<IPromptRepository> _promptRepository;
        private Mock<ICategoryRepository> _categoryRepository;
        private List<Prompt> _testingListOfPrompts;
        private List<Category> _testingListOfCategories;


        #region TestInitialize
        [TestInitialize]
        public void TestInitialize()
        {
            _cacheRepository = new Mock<ICacheService>();
            _search = new Mock<ISearchService>();
            _promptRepository = new Mock<IPromptRepository>();
            _categoryRepository = new Mock<ICategoryRepository>();

            _prompts = new PromptStorage.Prompts(_promptRepository.Object, _categoryRepository.Object, _search.Object, _cacheRepository.Object);
            _testingListOfCategories = new List<Category>()
            {
                new Category()
                {
                    ID=1,
                    Name="Important",
                    ParentCategory = new Category()
                },
                new Category()
                {
                    ID =2,
                    Name="Work",
                    ParentCategory = new Category(){ID=1}
                },
                new Category()
                {
                    ID=3,
                    Name="Family",
                     ParentCategory = new Category(){ID=1}
                },
                new Category()
                {
                    ID=4,
                    Name="Other",
                     ParentCategory=new Category()
                }
            };
            _testingListOfPrompts = new List<Prompt>()
            {
                new Prompt()
                {
                    ID=1,
                    userID=2,
                    Name="Make site start page",
                    Category=_testingListOfCategories[1],
                    Date=new System.DateTime(2018,06,15),
                    Description="Until it is June,15",
                    Actions=new List<Action>()
                    {
                        new Action()
                        {
                            ID=1,
                            Name="Make html"
                        },
                        new Action()
                        {
                            ID=2,
                            Name="Make css"
                        },
                        new Action()
                        {
                            ID=3,
                            Name="Make js"
                        }
                    },
                    Image="ImagePath1",
                    TimeOfPrompt=new System.TimeSpan(12,12,0)
                },
                new Prompt()
                {
                    ID=2,
                    userID=2,
                    Name="Buy clocks for father",
                    Category=_testingListOfCategories[2],
                    Date=new System.DateTime(2018,01,06),
                    Description="Now",
                    Actions=new List<Action>()
                    {
                        new Action()
                        {
                            ID=1,
                            Name="Find shop"
                        },
                        new Action()
                        {
                            ID=2,
                            Name="Buy"
                        }
                    },
                    Image="ImagePath2",
                    TimeOfPrompt=new System.TimeSpan(16,0,0)
                },
                new Prompt()
                {
                    ID =3,
                    userID=2,
                    Name="Paint picture",
                    Category=_testingListOfCategories[3],
                    Date=new System.DateTime(2018,10,06),
                    Description="For dining room",
                    Actions=new List<Action>()
                    {
                        new Action()
                        {
                            ID=1,
                            Name="Buy layout"
                        },
                        new Action()
                        {
                            ID=2,
                            Name="Paint something wonderful"
                        }
                    },
                    Image="ImagePath3",
                    TimeOfPrompt=new System.TimeSpan(15,30,0)

                },
            };






        }
        #endregion


        #region GetCategoryItemsByID

        [TestMethod]
        public void GetCategoryItemsByID_validInputData_NoUnderCategories_ListOfCategoryPrompts()
        {
            //
            //Arrange
            //
            int UserID = 2;
            int ID = 2;

            _categoryRepository.Setup(m => m.GetCategories(It.IsAny<int>())).Returns<IList<Category>>(null);
            _promptRepository.Setup(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>())).Returns(_testingListOfPrompts.Where(m => m.Category.ID == ID).ToList);


            //
            //Act
            //
            IList<Prompt> result = _prompts.GetCategoryItemsByID(UserID, ID);

            //
            //Assert
            //
            Assert.AreEqual(_testingListOfPrompts[0].Name, result[0].Name);
            _categoryRepository.Verify(m => m.GetCategories(It.IsAny<int>()), Times.Once);
            _promptRepository.Verify(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public void GetCategoryItemsByID_validInputData_UnderCategories_ListOfCategoryPrompts()
        {
            //
            //Arrange
            //
            int UserID = 2;
            int ID = 1;

            _categoryRepository.Setup(m => m.GetCategories(It.IsAny<int>())).Returns(_testingListOfCategories.Where(m => m.ParentCategory.ID == 1).ToList);
            _promptRepository.Setup(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>())).Returns<Prompt>(null);

            //
            //Act
            //
            IList<Prompt> result = _prompts.GetCategoryItemsByID(UserID, ID);

            //
            //Assert
            //
            Assert.AreEqual(0, result.Count);
            _categoryRepository.Verify(m => m.GetCategories(It.IsAny<int>()), Times.Once);
            _promptRepository.Verify(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public void GetCategoryItemsByID_validInputData_BothNullLists_Null()
        {
            //
            //Arrange
            //
            int UserID = 2;
            int ID = 5;

            _categoryRepository.Setup(m => m.GetCategories(It.IsAny<int>())).Returns<Category>(null);
            _promptRepository.Setup(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>())).Returns<Prompt>(null);

            //
            //Act
            //
            IList<Prompt> result = _prompts.GetCategoryItemsByID(UserID, ID);

            //
            //Assert
            //
            Assert.AreEqual(null, result);
            _categoryRepository.Verify(m => m.GetCategories(It.IsAny<int>()), Times.Once);
            _promptRepository.Verify(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public void GetCategoryItemsByID_InvalidInputData_Null()
        {
            //
            //Arrange
            //
            int UserID = -1;
            int ID = 0;
            //
            //Act
            //
            IList<Prompt> result = _prompts.GetCategoryItemsByID(UserID, ID);
            //
            //Assert
            //
            Assert.AreEqual(null, result);
            _categoryRepository.Verify(m => m.GetCategories(It.IsAny<int>()), Times.Never);
            _promptRepository.Verify(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        }
        #endregion


        #region GetPromptDetails



        [TestMethod]
        public void GetPromptDetails_InvalidID_Null()
        {
            //
            //Arrange
            //
            int userID = 2;
            int ID = -1;
            //
            //Act
            //
            Prompt result = _prompts.GetPromptDetails(userID, ID);
            //
            //Assert
            //
            Assert.AreEqual(null, result);
            _categoryRepository.Verify(m => m.GetCategories(It.IsAny<int>()), Times.Never);
            _promptRepository.Verify(m => m.GetPromptsList(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        }
        [TestMethod]
        public void GetPromptDetils_ValidID_NoCache_Prompt()
        {
            //
            //Arrange
            //
            int userID = 2;
            int ID = 1;
            _promptRepository.Setup(m => m.GetPrompt(It.IsAny<int>(), It.IsAny<int>())).Returns(_testingListOfPrompts.Where(m => m.ID == ID).FirstOrDefault);
            _cacheRepository.Setup(m => m.GetValueOfCache<Prompt>(It.IsAny<int>())).Returns<Prompt>(null);
            //
            //Act
            //
            Prompt result = _prompts.GetPromptDetails(userID, ID);
            //
            //Assert
            //
            Assert.AreEqual(_testingListOfPrompts[0].Name, result.Name);
            _cacheRepository.Verify(c => c.GetValueOfCache<Prompt>(It.IsAny<int>()), Times.Once);
            _cacheRepository.Verify(c => c.AddCache<Prompt>(It.IsAny<Prompt>(), It.IsAny<int>()), Times.Once);
            _promptRepository.Verify(r => r.GetPrompt(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
        [TestMethod]
        public void GetPromptDetails_ValidID_Cache_Prompt()
        {
            //
            //Arrange
            //
            int userID = 2;
            int ID = 3;
            _promptRepository.Setup(m => m.GetPrompt(It.IsAny<int>(), It.IsAny<int>())).Returns(_testingListOfPrompts.Where(m => m.ID == ID).FirstOrDefault);
            _cacheRepository.Setup(m => m.GetValueOfCache<Prompt>(It.IsAny<int>())).Returns(_testingListOfPrompts.Where(m => m.ID == ID).FirstOrDefault);
            //
            //Act
            //
            Prompt result = _prompts.GetPromptDetails(userID, ID);
            //
            //Assert
            //
            Assert.AreEqual(_testingListOfPrompts[2].Name, result.Name);
            _cacheRepository.Verify(c => c.GetValueOfCache<Prompt>(It.IsAny<int>()), Times.Once);
            _cacheRepository.Verify(c => c.AddCache<Prompt>(It.IsAny<Prompt>(), It.IsAny<int>()), Times.Never);
            _promptRepository.Verify(r => r.GetPrompt(It.IsAny<int>(), It.IsAny<int>()), Times.Never);

        }



        #endregion


        #region GetSearchingPrompts

        [TestMethod]
        public void GetSearchingPrompts_InvalidInputdata_Null()
        {
            //
            //Arrange
            //
            string promptval = null;
            string categoryval = "2";
            string dateval = null;
            int userID = 0;
            //
            //Act
            //
            var result = _prompts.GetSearchingPrompts(userID, promptval, categoryval, dateval);
            //
            //Assert
            //
            Assert.AreEqual(null, result);
            _promptRepository.Verify(r => r.GetPrompt(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
        [TestMethod]
        public void GetSearchingPrompts_validInputdata_List()
        {
            //
            //Arrange
            //
            string promptval = null;
            string categoryval = "2";
            string dateval = null;
            int userID = 2;
            _search.Setup(m => m.GetSearchResult(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(_testingListOfPrompts.Where(g => g.Category.ID==int.Parse(categoryval)).ToList);
            //
            //Act
            //
            var result = _prompts.GetSearchingPrompts(userID, promptval, categoryval, dateval);
            //
            //Assert
            //
            Assert.AreEqual(1, result[0].ID);
            _search.Verify(r => r.GetSearchResult(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
        #endregion
    }
}
