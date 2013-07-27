using System.Collections.Generic;
using RexConnectClient.Core;
using RexConnectClient.Core.Transfer;
using Weaver.Core.Query;
using Weaver.Core.Util;

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
		public RequestCmd AddQuery(string pScript, IDictionary<string, IWeaverQueryVal> pParams) {
			string[] args = WeaverUtil.GetScriptAndParamJson(pScript, pParams);

			RequestCmd rc = new RequestCmd(RexConn.Command.Query.ToString().ToLower(), args);
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
		
		/*--------------------------------------------------------------------------------------------*/
		public IList<RequestCmd> AddQueries(IWeaverTransaction pTransaction, bool pAsSession) {
			return AddQueries(pTransaction.Queries, pAsSession);
		}

	}
	
}