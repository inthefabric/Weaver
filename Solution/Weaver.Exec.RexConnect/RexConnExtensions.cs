using RexConnectClient.Core;
using RexConnectClient.Core.Result;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public static class RexConnExtensions {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IResponseResult Execute(this WeaverRequest pRequest, string pHostName, int pPort){
			var ctx = new RexConnContext(pRequest, pHostName, pPort);
			var data = new RexConnDataAccess(ctx);
			return data.Execute();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IResponseResult Execute(this IWeaverScript pWeaverScript, string pHostName,
												int pPort, string pRequestId, string pSessionId=null) {
			var r = new WeaverRequest(pRequestId, pSessionId);
			r.AddQuery(pWeaverScript);
			return r.Execute(pHostName, pPort);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static IResponseResult ExecuteAsSession(this IWeaverTransaction pTransaction,
													string pHostName, int pPort, string pRequestId) {
			var r = new WeaverRequest(pRequestId);
			r.AddQueries(pTransaction, true);
			return r.Execute(pHostName, pPort);
		}

	}
	
}