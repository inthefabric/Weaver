using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public interface ICommandResult {

		ResponseCmd Command { get; }
		TextResultList TextResults { get; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IList<GraphElement> ToGraphElements();

	}

}