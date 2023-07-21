using Google.Protobuf;
using System.Net;
using System.Net.Sockets;

namespace Cas
{
    namespace Extensions
    {
        public static class TcpClientExtensions
        {
            public static bool sendMessage(this TcpClient client, Google.Protobuf.IMessage dataMessage)
            {
                try
                {
                    byte[] data = dataMessage.ToByteArray();
                    client.GetStream().Write(data, 0, data.Length);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}