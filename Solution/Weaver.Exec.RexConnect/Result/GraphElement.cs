using System;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public class GraphElement : IGraphElement {

		public RexConn.GraphElementType Type { get; set; }
		public string Id { get; set; }
		public string Label { get; set; }
		public string OutVertexId { get; set; }
		public string InVertexId { get; set; }
		public JsonObject Properties { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static GraphElement Build(JsonObject pObj) {
			var ge = new GraphElement();
			ge.Id = pObj["_id"];
			ge.Properties = pObj.Object("_properties");

			switch ( pObj["_type"] ) {
				case "vertex":
					ge.Type = RexConn.GraphElementType.Vertex;
					break;

				case "edge":
					ge.Type = RexConn.GraphElementType.Edge;
					ge.Label = pObj["_label"];
					ge.OutVertexId = pObj["_outV"];
					ge.InVertexId = pObj["_inV"];
					break;

				default:
					throw new Exception("Unknown GraphElementType: "+pObj["_type"]);
			}

			return ge;
		}

	}

}