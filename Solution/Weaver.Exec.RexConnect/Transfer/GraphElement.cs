﻿using System;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class GraphElement {

		public virtual RexConn.GraphElementType Type { get; set; }
		public virtual string Id { get; set; }
		public virtual string Label { get; set; }
		public virtual string InVertexId { get; set; }
		public virtual string OutVertexId { get; set; }
		public virtual JsonObject Properties { get; set; }

		public virtual string Message { get; set; }
		public virtual string Exception { get; set; }


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
					ge.Type = RexConn.GraphElementType.Error;
					throw new Exception("Unknown GraphElementType: "+pObj["_type"]);
			}

			return ge;
		}

	}

}