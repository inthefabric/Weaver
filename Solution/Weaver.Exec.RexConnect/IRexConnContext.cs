using System;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public interface IRexConnContext {

		string RequestId { get; }
		string HostName { get; }
		int Port { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Log(string pType, string pCategory, string pText, Exception pException=null);

	}

}