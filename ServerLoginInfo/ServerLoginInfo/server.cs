using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using Newtonsoft.Json;

namespace Project_Login
{
    class Server
    {
        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 4141);
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();

            Console.WriteLine(@"Server Started /n at {0}:{1}", endPoint.Address, endPoint.Port);

            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                message = cleanMessage(buffer);

                Person person = JsonConvert.DeserializeObject<Person>(message);

                byte[] bytes = System.Text.Encoding.Unicode.GetBytes("Your message was sent, " + person.Username);
                sender.GetStream().Write(bytes, 0, bytes.Length);

                sendEmail(person);
            }
        }

        private static void sendEmail(Person p)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("<smtp-server>", 25))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential("<email-address>", "<pass>");

                    client.Send(
                        new MailMessage("<your-email>", p.Password,
                            "Thank you for using this Services",
                            string.Format(@"Thank you for using this Services, {0}.   
                                We have recieved your message, '{1}'.", p.Username
                            )
                        )
                    );

                    Console.WriteLine("Email sent to " + p.Password);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != 0)
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
