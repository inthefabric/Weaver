using System;
using Weaver.Exec.RexConnect.Result;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {
	
	/*================================================================================================*/
	public class RexConnContext : IRexConnContext {

		public Request Request { get; private set; }
		public string HostName { get; private set; }
		public int Port { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RexConnContext(Request pRequest, string pHostName, int pPort) {
			Request = pRequest;
			HostName = pHostName;
			Port = pPort;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IResponseResult CreateResponseResult() {
			return new ResponseResult(this);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IRexConnTcp CreateTcpClient() {
			var tcp = new RexConnTcp(HostName, Port);
			tcp.SendBufferSize = tcp.ReceiveBufferSize = 1<<16;
			return tcp;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void Log(string pType, string pCategory, string pText,Exception pException=null){
			Console.WriteLine(pType+" | "+pCategory+" | "+pText+
				(pException == null ? "" : " | "+pException));
		}

	}

}