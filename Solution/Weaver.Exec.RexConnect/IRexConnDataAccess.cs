using Weaver.Exec.RexConnect.Result;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public interface IRexConnDataAccess {

		IRexConnContext Context { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ResponseResult Execute();

	}

}