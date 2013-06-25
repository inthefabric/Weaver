using System.Collections.Generic;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class CommandResult {

		public virtual ResponseCmd Command { get; private set; }
		public virtual TextResultList TextResults { get; internal set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public CommandResult(ResponseCmd pCommand) {
			Command = pCommand;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IList<GraphElement> ToGraphElements() {
			var list = new List<GraphElement>();

			foreach ( JsonObject jo in Command.Results ) {
				list.Add(GraphElement.Build(jo));
			}

			return list;
		}

	}

}