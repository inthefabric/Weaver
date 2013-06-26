using System;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public interface IRexConnContext {

		Request Request { get; }
		string HostName { get; }
		int Port { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IRexConnTcp CreateTcpClient();

		/*--------------------------------------------------------------------------------------------*/
		void Log(string pType, string pCategory, string pText, Exception pException=null);

	}

}