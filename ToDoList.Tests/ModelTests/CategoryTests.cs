using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTests : IDisposable
  {
        public CategoryTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
        }
    //
    //    [TestMethod]
    //    public void GetAll_CategoriesEmptyAtFirst_0()
    //    {
    //      //Arrange, Act
    //      int result = Category.GetAll().Count;
    //
    //      //Assert
    //      Assert.AreEqual(0, result);
    //    }
    //
    //   [TestMethod]
    //   public void Equals_ReturnsTrueForSameName_Category()
    //   {
    //     //Arrange, Act
    //     Category firstCategory = new Category("Household chores");
    //     Category secondCategory = new Category("Household chores");
    //
    //     //Assert
    //     Assert.AreEqual(firstCategory, secondCategory);
    //   }
    //
    //   [TestMethod]
    //   public void Save_SavesCategoryToDatabase_CategoryList()
    //   {
    //     //Arrange
    //     Category testCategory = new Category("Household chores");
    //     testCategory.Save();
    //
    //     //Act
    //     List<Category> result = Category.GetAll();
    //     List<Category> testList = new List<Category>{testCategory};
    //
    //     //Assert
    //     CollectionAssert.AreEqual(testList, result);
    //   }
    //
    //
    //  [TestMethod]
    //  public void Save_DatabaseAssignsIdToCategory_Id()
    //  {
    //    //Arrange
    //    Category testCategory = new Category("Household chores");
    //    testCategory.Save();
    //
    //    //Act
    //    Category savedCategory = Category.GetAll()[0];
    //
    //    int result = savedCategory.GetId();
    //    int testId = testCategory.GetId();
    //
    //    //Assert
    //    Assert.AreEqual(testId, result);
    // }
    //
    //
    // [TestMethod]
    // public void Find_FindsCategoryInDatabase_Category()
    // {
    //   //Arrange
    //   Category testCategory = new Category("Household chores");
    //   testCategory.Save();
    //
    //   //Act
    //   Category foundCategory = Category.Find(testCategory.GetId());
    //
    //   //Assert
    //   Assert.AreEqual(testCategory, foundCategory);
    // }
    //
    // public void Dispose()
    // {
    //   Item.DeleteAll();
    //   Category.DeleteAll();
    // }

    [TestMethod]
    public void GetItems_RetrievesAllItemsWithCategory_ItemList()
    {
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Item firstItem = new Item("Mow the lawn");
      firstItem.Save();
      Item secondItem = new Item("Do the dishes");
      secondItem.Save();


      List<Item> testItemList = new List<Item> {firstItem, secondItem};
      List<Item> resultItemList = testCategory.GetItems();

      CollectionAssert.AreEqual(testItemList, resultItemList);
    }
    [TestMethod]
      public void GetItems_ReturnsAllCategoryItems_ItemList()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        Item testItem1 = new Item("Mow the lawn");
        testItem1.Save();

        Item testItem2 = new Item("Buy plane ticket");
        testItem2.Save();

        //Act
        testCategory.AddItem(testItem1);
        List<Item> savedItems = testCategory.GetItems();
        List<Item> testList = new List<Item> {testItem1};

        //Assert
        CollectionAssert.AreEqual(testList, savedItems);
      }
  }
}
