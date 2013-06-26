using System.Net.Sockets;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public class RexConnTcp : IRexConnTcp {

		private readonly TcpClient vTcpClient;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RexConnTcp(string pHostName, int pPort) {
			vTcpClient = new TcpClient(pHostName, pPort);
		}

		/*--------------------------------------------------------------------------------------------*/
		public int SendBufferSize {
			get { return vTcpClient.SendBufferSize; }
			set { vTcpClient.SendBufferSize = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public int ReceiveBufferSize {
			get { return vTcpClient.ReceiveBufferSize; }
			set { vTcpClient.ReceiveBufferSize = value; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public NetworkStream GetStream() {
			return vTcpClient.GetStream();
		}

	}

}