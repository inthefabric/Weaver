using System;
using System.Net.Sockets;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public class RexConnContext : IRexConnContext {

		public string RequestId { get; private set; }
		public string HostName { get; private set; }
		public int Port { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RexConnContext(string pRequestId, string pHostName, int pPort) {
			RequestId = pRequestId;
			HostName = pHostName;
			Port = pPort;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual TcpClient CreateTcpClient() {
			var tcp = new TcpClient(HostName, Port);
			tcp.SendBufferSize = tcp.ReceiveBufferSize = 1<<16;
			return tcp;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Log(string pType, string pCategory, string pText,Exception pException=null){
			Console.WriteLine(pType+" | "+pCategory+" | "+pText+
				(pException == null ? "" : " | "+pException));
		}

	}

}