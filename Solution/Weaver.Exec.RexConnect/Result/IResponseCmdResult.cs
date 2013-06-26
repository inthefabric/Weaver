using System.Collections.Generic;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public interface IResponseCmdResult {

		ResponseCmd Command { get; }
		TextResultList TextResults { get; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IList<GraphElement> ToGraphElements();

	}

}