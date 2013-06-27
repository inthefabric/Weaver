using RexConnectClient.Core;
using RexConnectClient.Core.Result;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public static class RexConnExtensions {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IResponseResult Execute(this IWeaverScript pWeaverScript, string pHostName,
												int pPort, string pRequestId, string pSessionId=null) {
			var r = new WeaverRequest(pRequestId, pSessionId);
			r.AddQuery(pWeaverScript);

			var ctx = new RexConnContext(r, pHostName, pPort);
			var data = new RexConnDataAccess(ctx);
			return data.Execute();
		}

	}
	
}