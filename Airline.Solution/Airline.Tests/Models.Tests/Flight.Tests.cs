using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Airline.Models;

namespace Airline.Models.Tests
{
  [TestClass]
  public class FlightTests : IDisposable
  {
    public FlightTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }
    public void Dispose()
    {
      City.ClearAll();
      Flight.ClearAll();
    }

    [TestMethod]
    public void GetAll_CheckIfDatabaseIsEmpty_0()
    {
      int cities = Flight.GetAll().Count;
      Assert.AreEqual(0, cities);
    }

    [TestMethod]
    public void Equals_CheckIfEqualsOverrideWorks_City()
    {
      DateTime dummyDateTime = new DateTime(2016,12,31);
      Flight flight1 = new Flight(dummyDateTime, "Portland", "Bozeman", "On-time");
      Flight flight2 = new Flight(dummyDateTime, "Portland", "Bozeman", "On-time");

      Assert.AreEqual(flight1, flight2);
    }
    [TestMethod]
    public void Save_SavesFlightsToDatabase_FlightList()
    {
      DateTime newDateTime = new DateTime(2016,12,31);
      Flight testFlight = new Flight(newDateTime, "Portland", "Bozeman", "On-time");
      testFlight.Save();

      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};

      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToObject_Id()
    {
      DateTime newDateTime = new DateTime(2016,12,31);
      Flight testFlight = new Flight(newDateTime, "Portland", "Bozeman", "On-time");
      testFlight.Save();

      Flight savedFlight = Flight.GetAll()[0];

      int result = savedFlight.Id;
      int testId = testFlight.Id;

      Assert.AreEqual(testId, result);
    }
    [TestMethod]
    public void Find_FindsFlightInDatabase_Flight()
    {
      //Arrange
      DateTime newDateTime = new DateTime(2016,12,31);
      Flight testFlight = new Flight(newDateTime, "Portland", "Bozeman", "On-time");
      testFlight.Save();

      //Act
      Flight foundFlight = Flight.Find(testFlight.Id);

      //Assert
      Assert.AreEqual(testFlight, foundFlight);
    }
    [TestMethod]
    public void AddCity_AddsCityToFlight_FlightList()
    {
      DateTime newDateTime = new DateTime(2016,12,31);
      Flight testFlight = new Flight(newDateTime, "Portland", "Bozeman", "On-time");
      testFlight.Save();

      City testCity = new City("Des Moines");
      testCity.Save();

      testFlight.AddCity(testCity);

      List<City> result = testFlight.GetCities();
      List<City> testList = new List<City>{testCity};

      CollectionAssert.AreEqual(testList, result);
    }
  }
}
