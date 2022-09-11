using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Project_Login_Client
{
    class Client
    {
        static void Main(string[] args)
        {


            Console.Write("Enter your username: ");
            string name = Console.ReadLine();
            Console.Write("Enter your password: ");
            string password = Console.ReadLine();

            var personJson = new Person
            {
                Username = name,
                Password = password
            };

            string jsonString = JsonSerializer.Serialize(personJson);

            // Send the message  
            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(jsonString));
        }

        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;
            try 
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 4141); 
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); 
                Console.WriteLine("================================");
                Console.WriteLine("=   Connected to the server    =");
                Console.WriteLine("================================");
                Console.WriteLine("Waiting for response...");

                messageBytes = new byte[bytesize]; 

                stream.Read(messageBytes, 0, messageBytes.Length);

            }
            catch (Exception e)  
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes; 
        }

    }

    class Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
