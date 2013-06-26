using System;
using System.Net.Sockets;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public interface IRexConnContext {

		string RequestId { get; }
		string HostName { get; }
		int Port { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		TcpClient CreateTcpClient();

		/*--------------------------------------------------------------------------------------------*/
		void Log(string pType, string pCategory, string pText, Exception pException=null);

	}

}