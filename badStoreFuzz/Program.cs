using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace badStoreFuzz
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] requestLine = File.ReadAllLines(args[0]);
            string[] parameters = requestLine[requestLine.Length - 1].Split('&');
            string host = string.Empty;
            StringBuilder requestBuilder = new StringBuilder();

            foreach (string line in requestLine) {
                if (line.StartsWith("Host: ")) {
                    host = line.Split(' ')[1].Replace("\r", string.Empty);
                }
                requestBuilder.Append(line + "\n");
            }

            string request = requestBuilder.ToString() + "\r\n";
            System.Console.WriteLine(request);

            System.Console.WriteLine(host);
            IPEndPoint rhost = new IPEndPoint(IPAddress.Parse(host), 80);
            foreach (string parameter in parameters) {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                    socket.Connect(rhost);

                    string value = parameter.Split('=')[1];
                    string req = request.Replace("=" + value, "=" + value + "'");

                    byte[] requestBytes = Encoding.ASCII.GetBytes(req);
                    socket.Send(requestBytes);

                    byte[] buffer = new byte[socket.ReceiveBufferSize];

                    socket.Receive(buffer);
                    string response = Encoding.ASCII.GetString(buffer);
                    if (response.Contains("error in your SQL syntax")) {
                        System.Console.WriteLine("Parameter " + parameter + " seems vulnerable.");
                        System.Console.WriteLine(" to SQL injection with value: " + value + "'");
                    }
                }
            }
        
        }
    }
}
