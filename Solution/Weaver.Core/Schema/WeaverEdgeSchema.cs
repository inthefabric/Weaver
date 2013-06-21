using Weaver.Core.Elements;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Schema {

	/*================================================================================================*/
	public class WeaverEdgeSchema : WeaverItemSchema {

		public WeaverVertexSchema OutVertex { get; private set; }
		public WeaverVertexSchema InVertex { get; private set; }
		public string EdgeType { get; private set; }

		private WeaverEdgeConn vOutVertexConn;
		private WeaverEdgeConn vInVertexConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeSchema(WeaverVertexSchema pOutVertex, string pName, string pDbName, 
								string pEdgeType, WeaverVertexSchema pInVertex) : base(pName, pDbName) {
			OutVertex = pOutVertex;
			InVertex = pInVertex;
			EdgeType = pEdgeType;

			OutVertexConn = WeaverEdgeConn.OutZeroOrMore;
			InVertexConn = WeaverEdgeConn.InZeroOrMore;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeConn OutVertexConn {
			get {
				return vOutVertexConn;
			}
			set {
				vOutVertexConn = value;

				if ( !WeaverEdge.IsConnOutgoing(vOutVertexConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an incoming "+
						"OutVertexConn value.");
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeConn InVertexConn {
			get {
				return vInVertexConn;
			}
			set {
				vInVertexConn = value;

				if ( WeaverEdge.IsConnOutgoing(vInVertexConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an outgoing "+
						"InVertexConn value.");
				}
			}
		}

	}

}