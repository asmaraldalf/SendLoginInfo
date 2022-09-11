using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Project_Login_Client
{
    class Client
    {
        static void Main(string[] args)
        {
            Person person = new Person();

            Console.WriteLine("Enter your name: ");
            person.Name = Console.ReadLine();
            Console.WriteLine("Enter your email address: ");
            person.Email = Console.ReadLine();

            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(person.ToJson()));
            Console.WriteLine(cleanMessage(bytes));

            Console.Read();
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

                stream.Dispose();
                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes;
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);
            Console.WriteLine(message);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }

            return messageToPrint;
        }
    }

    class Person
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public string ToJson()
        {
            string data = "{";
            data += "'name': '" + Name + "',";
            data += "'email': '" + Email + "',";
            data += "}";
            return data;
        }
    }
}
