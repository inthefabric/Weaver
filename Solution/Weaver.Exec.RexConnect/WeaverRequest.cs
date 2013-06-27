using System.Collections.Generic;
using RexConnectClient.Core;
using RexConnectClient.Core.Transfer;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public class WeaverRequest : Request {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverRequest() {}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverRequest(string pRequestId) : base(pRequestId) {}
		
		/*--------------------------------------------------------------------------------------------*/
		public WeaverRequest(string pRequestId, string pSessionId) : base(pRequestId, pSessionId) {}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RequestCmd AddQuery(string pScript, IDictionary<string,IWeaverQueryVal> pParams) {
			RequestCmd rc = WeaverRequestCmd.CreateQueryCommand(pScript, pParams);
			CmdList.Add(rc);
			return rc;
		}

		/*--------------------------------------------------------------------------------------------*/
		public RequestCmd AddQuery(IWeaverScript pWeaverScript) {
			return AddQuery(pWeaverScript.Script, pWeaverScript.Params);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<RequestCmd> AddQueries(IEnumerable<IWeaverScript> pWeaverScripts, bool pAsSession){
			var list = new List<RequestCmd>();
			RequestCmd rc;

			if ( pAsSession ) {
				rc = AddSessionAction(RexConn.SessionAction.Start);
				list.Add(rc);
			}

			foreach ( IWeaverScript ws in pWeaverScripts ) {
				rc = AddQuery(ws);
				list.Add(rc);
			}

			if ( pAsSession ) {
				rc = AddSessionAction(RexConn.SessionAction.Commit);
				list.Add(rc);

				rc = AddSessionAction(RexConn.SessionAction.Close);
				list.Add(rc);
			}

			return list;
		}

	}
	
}