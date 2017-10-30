using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airline.Models;

namespace Airline.Models.Tests
{
  [TestClass]
  public class CityTests : IDisposable
  {
    public CityTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }
    public void Dispose()
    {
      City.ClearAll();
    }
    [TestMethod]
    public void GetAll_CheckIfDatabaseIsEmpty_0()
    {
      int cities = City.GetAll().Count;
      Assert.AreEqual(0, cities);
    }
    [TestMethod]
    public void Equals_CheckIfEqualsOverrideWorks_City()
    {
      City city1 = new City("Portland");
      City city2 = new City("Portland");

      Assert.AreEqual(city1, city2);
    }
    [TestMethod]
    public void Save_SavesCityToDatabase_CityList()
    {
      City testCity = new City("Portland");
      testCity.Save();

      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToCategory_Id()
    {
      City testCity = new City("Portland");
      testCity.Save();

      City savedCity = City.GetAll()[0];

      int result = savedCity.Id;
      int testId = testCity.Id;

      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsCityInDatabase_City()
    {
      City testCity = new City("Portland");
      testCity.Save();

      City foundCity = City.Find(testCity.Id);

      Assert.AreEqual(testCity, foundCity);
    }
    // [TestMethod]
    // public void Delete_DeletesCityInDatabase_0()
    // {
    //   // City testCity = new City("Portland");
    //   // testCity.Save();
    // }
    
  }
}
