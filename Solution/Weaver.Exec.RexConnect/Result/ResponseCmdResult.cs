using System.Collections.Generic;
using ServiceStack.Text;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public class ResponseCmdResult : IResponseCmdResult {

		public ResponseCmd Command { get; protected set; }
		public TextResultList TextResults { get; internal set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ResponseCmdResult(ResponseCmd pCommand) {
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