using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        static int port = 8005; // порт для прийому вхідних повідомлень

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // 1. створення сокета
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // отримуємо адресу для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);


            try
            {
                // звязуємо сокет с локальною точкою, по якій будемо прийимати дані
                socket.Bind(ipPoint);

                // починаємо прослуховування
                socket.Listen(10);
                Console.WriteLine("Сервер запустився. Очікування підключень...");

                //замкнутий цикл
                while (true)
                {
                    Socket handler = socket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[1024]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());

                    // отправляем ответ
                    string message = "ваше сообщение доставлено";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
