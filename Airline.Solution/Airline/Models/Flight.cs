using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Airline.Models
{
  public class Flight
  {
    public int Id{get; private set;}
    public DateTime DepartureTime{get;}
    public string DepartureCity{get;}
    public string DestinationCity{get;}
    public string Status{get;}

    public Flight(DateTime departureTime, string departureCity, string destinationCity, string status, int id = 0)
    {
      DepartureTime = departureTime;
      DepartureCity = departureCity;
      DestinationCity = destinationCity;
      Status = status;
      Id = id;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = this.Id == newFlight.Id;
        bool departureTimeEquality = this.DepartureTime == newFlight.DepartureTime;
        bool departureCityEquality = this.DepartureCity == newFlight.DepartureCity;
        bool destinationCityEquality = this.DestinationCity == newFlight.DestinationCity;
        bool statusEquality = this.Status == newFlight.Status;
        return (idEquality && departureTimeEquality && departureCityEquality && destinationCityEquality && statusEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.DepartureTime.GetHashCode();
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM flights;";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (departure_time, departure_city, destinations, status) VALUES (@departure_time, @departure_city, @destinations, @status);";

      MySqlParameter departureTime = new MySqlParameter();
      departureTime.ParameterName = "@departure_time";
      departureTime.Value = this.DepartureTime.ToString("yyyy-MM-dd HH:mm:ss");
      cmd.Parameters.Add(departureTime);

      MySqlParameter departureCity = new MySqlParameter();
      departureCity.ParameterName = "@departure_city";
      departureCity.Value = this.DepartureCity;
      cmd.Parameters.Add(departureCity);

      MySqlParameter destinations = new MySqlParameter();
      destinations.ParameterName = "@destinations";
      destinations.Value = this.DestinationCity;
      cmd.Parameters.Add(destinations);

      MySqlParameter status = new MySqlParameter();
      status.ParameterName = "@status";
      status.Value = this.Status;
      cmd.Parameters.Add(status);

      cmd.ExecuteNonQuery();
      Id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        DateTime flightDepartureT = rdr.GetDateTime(1);
        string flightDepartureC = rdr.GetString(2);
        string flightDestination = rdr.GetString(3);
        string flightStatus = rdr.GetString(4);
        Flight newFlight = new Flight(flightDepartureT, flightDepartureC, flightDestination, flightStatus, flightId);
        allFlights.Add(newFlight);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText =@"SELECT * FROM flights WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      string flightDepartureC = "";
      string flightDestination = "";
      string flightStatus = "";
      DateTime flightDepartureT = new DateTime();;

      while(rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        flightDepartureT = rdr.GetDateTime(1);
        flightDepartureC = rdr.GetString(2);
        flightDestination = rdr.GetString(3);
        flightStatus = rdr.GetString(4);
      }
      Flight newFlight = new Flight(flightDepartureT, flightDepartureC, flightDestination, flightStatus, flightId);
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
      return newFlight;
    }
    public void AddCity(City newCity)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities_flights (cities_id, flights_id) VALUES (@CityId, @FlightId);";

      MySqlParameter cityId = new MySqlParameter();
      cityId.ParameterName = "@CityId";
      cityId.Value = newCity.Id;
      cmd.Parameters.Add(cityId);

      MySqlParameter flightId = new MySqlParameter();
      flightId.ParameterName = "@FlightId";
      flightId.Value = Id;
      cmd.Parameters.Add(flightId);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
    public List<City> GetCities()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT cities.* FROM flights JOIN cities_flights ON (flights.id = cities_flights.flights_id) JOIN cities ON (cities_flights.cities_id = cities.id) WHERE flights.id = @FlightId;";

      MySqlParameter FlightsId = new MySqlParameter();
      FlightsId.ParameterName = "@FlightId";
      FlightsId.Value = Id;
      cmd.Parameters.Add(FlightsId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<City> allCities = new List<City>{};

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }
  }
}
