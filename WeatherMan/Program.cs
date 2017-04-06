using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WeatherMan
{
    class Program 
    {
        public static void StartProgram()
        {

            Console.WriteLine("Yo whats cho name B?");
            string Name = Console.ReadLine();
            Console.WriteLine($"Yo {Name} whats yo zip code");
            var Zip = Console.ReadLine();

            var url = "http://api.openweathermap.org/data/2.5/weather?zip="+ Zip +",us&units=imperial&id=524901&APPID=2dced56eb55ad80cee25292332513642";

            var request = WebRequest.Create(url);

            var response = request.GetResponse();

            var rawResponse = String.Empty;
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                rawResponse = reader.ReadToEnd();
                
            }

            var GettoFohCast = JsonConvert.DeserializeObject<RootObject>(rawResponse);

            Console.WriteLine($"Its {GettoFohCast.Main.Temp}°F outside cuz");
            Console.WriteLine($"And Its {GettoFohCast.Weather.First().Description} TODAY!!! Little Bruh");


            Program.DataSender(Name,GettoFohCast);

        }
        
        public static void DataSender(string Name,RootObject GettoFohCast)
        {


            const string connectionString =
                  @"Server=localhost\SQLEXPRESS;Database=WeatherHood;Trusted_Connection=True;";

            using (var connection = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected!");
                var sqlCommand = new SqlCommand();
                sqlCommand.Connection = connection;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                Console.WriteLine("connected");

                var text = @"INSERT INTO WeaterInTheHood (UserName, CurrentCondition, Temperature)" +
                "Values (@UserName, @CurrentCondition, @Temperature)";
                var cmd = new SqlCommand(text, connection);
                cmd.Parameters.AddWithValue("@UserName", Name);
                cmd.Parameters.AddWithValue("@CurrentCondition", (GettoFohCast.Weather[0].Description));
                cmd.Parameters.AddWithValue("@Temperature", (GettoFohCast.Main.Temp));
                connection.Open();
                cmd.ExecuteNonQuery();

                connection.Close();
            }



        }

        static void Main(string[] args)
        {
            Program.StartProgram();

        }





    }
}
 