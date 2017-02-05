using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;

namespace Demo01.Controllers
{
    public class WSController : ApiController
    {
        [Route("WS/Get")]
        public IHttpActionResult Get()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(ProcessWS);
            }
            return StatusCode(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessWS(AspNetWebSocketContext arg)
        {
            WebSocket socket = arg.WebSocket;
            while (true)
            {
                ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
                WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socket.State == WebSocketState.Open)
                {
                    string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                    string returnMsg = GetReturnMsg(message);
                    buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(returnMsg));
                    await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                }else
                {
                    break;
                }
            }
        }

        private string GetReturnMsg(string msg)
        {
            return $"hello,{msg}";
        }
    }
}
