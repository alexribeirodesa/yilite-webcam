using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace YiWebcam {
    class Program {
        private static string _yiIP = "192.168.42.1";
        private static int _yiPort = 7878;
        private static Socket _yiSocket;
        private static int _yiToken = 0;
        private static bool _yiFirst = true;
        private static byte[] _yiData = new byte[512];
        static void Main(string[] args) {
            // CONNECT TO YOUR YI CAMERA
            _yiSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Establishing Connection to {0}", _yiIP);
            _yiSocket.Connect(_yiIP, _yiPort);
            Console.WriteLine("Connection established!");

            _yiSocket.Send(Encoding.UTF8.GetBytes("{\"msg_id\":257,\"token\":0}"));

            bool done = false;

            while (!done) {
                if (GetToken()) {
                } else { 
                    GetToken();
                }

                /*
                Array.Clear(_yiData, 0, _yiData.Length);
                _yiSocket.Receive(_yiData);
                string d = System.Text.Encoding.UTF8.GetString(_yiData);

                if (d.Contains("rval")) {
                    Regex rx = new Regex("\"param\":(\\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    MatchCollection matches = rx.Matches(d);
                    if (matches.Count > 0) {
                        _yiToken = Int32.Parse(matches[0].Value.Replace("\"param\":", ""));
                    }
                } else {
                    Array.Clear(_yiData, 0, _yiData.Length);
                    _yiSocket.Receive(_yiData);
                    d = System.Text.Encoding.UTF8.GetString(_yiData);

                    if (d.Contains("rval")) {
                        Regex rx = new Regex("\"param\":(\\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        MatchCollection matches = rx.Matches(d);
                        if (matches.Count > 0) {
                            _yiToken = Int32.Parse(matches[0].Value.Replace("\"param\":", ""));
                        }
                    }
                }*/


                _yiSocket.Send(Encoding.UTF8.GetBytes("{\"msg_id\":259,\"token\":" + _yiToken + ",\"param\":\"none_force\"}"));
                Thread.Sleep(1000);
            }
        }

        static bool GetToken() {
            Array.Clear(_yiData, 0, _yiData.Length);
            _yiSocket.Receive(_yiData);
            string d = System.Text.Encoding.UTF8.GetString(_yiData);

            if (d.Contains("rval")) {
                Regex rx = new Regex("\"param\":(\\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                MatchCollection matches = rx.Matches(d);
                if (matches.Count > 0) {
                    _yiToken = Int32.Parse(matches[0].Value.Replace("\"param\":", ""));
                }
                return true;
            }

            return false;
        }
    }
}