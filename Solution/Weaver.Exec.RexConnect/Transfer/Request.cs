using System.Collections.Generic;
using ServiceStack.Text;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class Request {

		public static bool CreateRequestsInDebugMode;

		public string ReqId { get; set; }
		public string SessId { get; set; }
		public IList<RequestCmd> CmdList { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public Request(string pRequestId) {
			ReqId = pRequestId;
			CmdList = new List<RequestCmd>();

			if ( CreateRequestsInDebugMode ) {
				CmdList.Add(RequestCmd.CreateConfigCommand(RexConn.ConfigSetting.Debug, "1"));
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Request(string pRequestId, string pScript, IDictionary<string,IWeaverQueryVal> pParams) :
																					this(pRequestId) {
			CmdList.Add(RequestCmd.CreateQueryCommand(pScript, pParams));
		}

		/*--------------------------------------------------------------------------------------------*/
		public Request(string pRequestId, string pScript) : this(pRequestId, pScript, null) {}

		/*--------------------------------------------------------------------------------------------*/
		public Request(string pRequestId, IWeaverScript pWeaverScript) :
										this(pRequestId, pWeaverScript.Script, pWeaverScript.Params) {}

		/*--------------------------------------------------------------------------------------------*/
		public Request(string pRequestId, IEnumerable<IWeaverScript> pWeaverScripts,
															bool pAsSession=true) : this(pRequestId) {
			if ( pAsSession ) {
				CmdList.Add(RequestCmd.CreateSessionCommand(RexConn.SessionAction.Start));
			}

			foreach ( IWeaverScript ws in pWeaverScripts ) {
				CmdList.Add(RequestCmd.CreateQueryCommand(ws.Script, ws.Params));
			}

			if ( pAsSession ) {
				CmdList.Add(RequestCmd.CreateSessionCommand(RexConn.SessionAction.Commit));
				CmdList.Add(RequestCmd.CreateSessionCommand(RexConn.SessionAction.Close));
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string ToRequestJson() {
			JsConfig.EmitCamelCaseNames = true;
			string json = JsonSerializer.SerializeToString(this);
			JsConfig.EmitCamelCaseNames = false;
			return json;
		}

	}
	
}