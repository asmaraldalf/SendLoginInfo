using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;

namespace TcpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 4141);
            TcpListener listener = new TcpListener(ep);
            listener.Start();

            Console.WriteLine(@"  
===================================================
Started listening requests at: {0}:{1}  
===================================================",
            ep.Address, ep.Port);

            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                message = cleanMessage(buffer);

                Person person = JsonConvert.DeserializeObject<Person>(message); 

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes("Thank you for your message, " + person.Username);
                sender.GetStream().Write(bytes, 0, bytes.Length); 

                Console.WriteLine($"client username : {person.Username} , client password : {person.Password}");
            }
        }


        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

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
        public string Username { get; set; }
        public string Password { get; set; }
    }
}