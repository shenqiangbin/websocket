using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    //自己写的监听843端口的程序，返回的始终失败
    class Program
    {
        static void Main(string[] args)
        {
            Socket s = null;
            try
            {
                int port = 843;
                string host = "127.0.0.1";
                IPAddress ip = IPAddress.Parse(host);
                IPEndPoint ipe = new IPEndPoint(ip, port);
                s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket类
                s.Bind(ipe);//绑定2000端口
                s.Listen(0);//开始监听
                //Console.WriteLine("Wait for connect");
                while (true)
                {
                    Socket temp = s.Accept();//为新建连接创建新的Socket。
                                             //Console.WriteLine("Get a connect");
                    string recvStr = "";
                    byte[] recvBytes = new byte[1024];
                    int bytes;
                    bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
                    recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
                    Console.WriteLine("Server Get Message:{0}", recvStr);//把客户端传来的信息显示出来 

                    byte[] bs = Encoding.UTF8.GetBytes(@"<?xml version='1.0' encoding='UTF-8'?>
	<cross-domain-policy xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' 
			    xsi:noNamespaceSchemaLocation='http://www.adobe.com/xml/schemas/PolicyFileSocket.xsd'>
	<allow-access-from domain='*' to-ports='*' secure='false' />
</cross-domain-policy>");
                    //context.Response.ContentType = "text/*";
                    //context.Response.OutputStream.Write(status, 0, status.Length);

                    //string sendStr = "Ok!Client Send Message Sucessful!";
                    //byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                    temp.Send(bs, bs.Length, 0);//返回客户端成功信息
                    temp.Close();                    
                }
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                s.Close();
            }
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
    }
}
