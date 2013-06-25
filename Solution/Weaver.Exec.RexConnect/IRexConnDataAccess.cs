using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public interface IRexConnDataAccess {

		IRexConnContext ApiCtx { get; }

		Request Request { get; }
		string RequestJson { get; }

		Response Response { get; }
		string ResponseJson { get; }
		int ExecutionMilliseconds { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void Execute();

	}

}