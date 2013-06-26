using System.Collections.Generic;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public interface IResponseResult {
		
		IRexConnContext Context { get; }

		Request Request { get; }
		string RequestJson { get; }

		Response Response { get; }
		string ResponseJson { get; }
		IList<ResponseCmdResult> CommandResults { get; }

		bool IsError { get; }
		int ExecutionMilliseconds { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetResponseJson(string pResponseJson);

		/*--------------------------------------------------------------------------------------------*/
		void SetResponseError(string pErr);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FillCommandTextResults();

	}

}