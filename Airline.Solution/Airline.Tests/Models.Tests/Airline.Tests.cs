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
    public City()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }
    public void Dispose()
    {
      Class.ClearAll
    }
    [TestMethod]
    public void Method_Description_ExpectedValue()
    {
      Assert.AreEqual(var1, method(input));
    }
  }
}
