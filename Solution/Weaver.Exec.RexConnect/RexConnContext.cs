using System;

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
		public virtual void Log(string pType, string pCategory, string pText,Exception pException=null){
			Console.WriteLine(pType+" | "+pCategory+" | "+pText+
				(pException == null ? "" : " | "+pException));
		}

	}

}