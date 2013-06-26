using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public interface IResponseResult {
		
		IRexConnContext Context { get; }

		Request Request { get; }
		string RequestJson { get; }

		Response Response { get; }
		string ResponseJson { get; }
		IList<CommandResult> CommandResults { get; }

		bool IsError { get; }
		int ExecutionMilliseconds { get; set; }



		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetRequest(IRexConnContext pContext, Request pRequest);

		/*--------------------------------------------------------------------------------------------*/
		void SetResponseJson(string pResponseJson);

		/*--------------------------------------------------------------------------------------------*/
		void SetErrorResponse(string pErr);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FillCommandTextResults();

	}

}