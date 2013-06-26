using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public interface IGraphElement {

		RexConn.GraphElementType Type { get; set; }
		string Id { get; set; }
		string Label { get; set; }
		string InVertexId { get; set; }
		string OutVertexId { get; set; }
		JsonObject Properties { get; set; }

		string Message { get; set; }
		string Exception { get; set; }

	}

}