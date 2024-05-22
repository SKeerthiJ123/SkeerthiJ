using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SpeedyAirly
{
    class Order
    {
        public string Destination { get; set; }
    }

    class Flight
    {
        public int FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Day { get; set; }
        public List<KeyValuePair<string, Order>> Orders { get; set; } = new List<KeyValuePair<string, Order>>(); // List to store scheduled orders

        public bool CanAccommodateOrder()
        {
            // Check if the number of scheduled orders is less than the capacity of the flight
            return Orders.Count < 20; // Assuming capacity of 20 boxes per flight
        }
        public override string ToString()
        {
            return $"Flight: {FlightNumber}, departure: {Departure}, arrival: {Arrival}, day: {Day}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Load flight schedule
            Flight[] flights = LoadFlightSchedule();
            Console.WriteLine("Flight Schedule Display:");
            foreach (var flight in flights)
            {
                Console.WriteLine(flight);
                
            }

            // Load orders from JSON file
            Dictionary<string, Order> orders = LoadOrdersFromJson("coding-assigment-orders.json");

            // Schedule orders onto flights
            ScheduleOrders(orders, flights);

            // Display flight itineraries
            Console.WriteLine("*****************************************************");
            Console.WriteLine("Flight Itenaries");
            foreach (var flight in flights)
            {
                foreach (var order in flight.Orders)
                {
                    Console.WriteLine($"    Order: {order.Key},Flight {flight.FlightNumber}, departure: {flight.Departure},arrival: {flight.Arrival},day: {flight.Day}");
                }
                Console.WriteLine("*****************************************************");
                if (flight.Orders.Count == 0)
                {
                    //Console.WriteLine($"Flight {flight.FlightNumber}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
                    //else
                    Console.WriteLine($"Flight{flight.FlightNumber}: not scheduled");
                }
                
                                
            }
        }

        static Flight[] LoadFlightSchedule()
        {
            // Define the flight schedule (hardcoded for this example)
            Flight[] flights = new Flight[]
            {
                new Flight { FlightNumber = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 },
                new Flight { FlightNumber = 2, Departure = "YUL", Arrival = "YYC", Day = 1 },
                new Flight { FlightNumber = 3, Departure = "YUL", Arrival = "YVR", Day = 1 },
                new Flight { FlightNumber = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 },
                new Flight { FlightNumber = 5, Departure = "YUL", Arrival = "YYC", Day = 2 },
                new Flight { FlightNumber = 6, Departure = "YUL", Arrival = "YVR", Day = 2 }
            };

            return flights;
        }

        static Dictionary<string, Order> LoadOrdersFromJson(string filePath)
        {
            // Read JSON file
            string jsonContent = System.IO.File.ReadAllText(filePath);

            // Deserialize JSON to dictionary<string, Order>
            Dictionary<string, Order> orders = JsonConvert.DeserializeObject<Dictionary<string, Order>>(jsonContent);

            return orders;
        }

        static void ScheduleOrders(Dictionary<string, Order> orders, Flight[] flights)
        {
            int count = 20;
            foreach (var order in orders)
            {
                bool scheduled = false;

                foreach (var flight in flights)
                {
                    if (flight.CanAccommodateOrder())
                    {
                        flight.Orders.Add(order);
                        scheduled = true;
                        break;
                    }
                }

                if (!scheduled)
                {
                    Console.WriteLine($"Order: {order.Key}, flightNumber: not scheduled");
                }
            }
        }
    }
}
