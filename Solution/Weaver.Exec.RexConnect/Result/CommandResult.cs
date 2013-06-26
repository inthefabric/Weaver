using System.Collections.Generic;
using ServiceStack.Text;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public class CommandResult : ICommandResult {

		public ResponseCmd Command { get; protected set; }
		public TextResultList TextResults { get; internal set; }

		
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