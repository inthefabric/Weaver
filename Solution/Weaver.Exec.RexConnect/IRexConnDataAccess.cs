﻿using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public interface IRexConnDataAccess {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		ResponseResult Execute(IRexConnContext pContext, Request pRequest);

	}

}