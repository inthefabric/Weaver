using Weaver.Core.Elements;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Schema {

	/*================================================================================================*/
	public class WeaverEdgeSchema : WeaverItemSchema {

		public WeaverVertexSchema FromNode { get; private set; }
		public WeaverVertexSchema ToNode { get; private set; }
		public string EdgeType { get; private set; }

		private WeaverEdgeConn vFromNodeConn;
		private WeaverEdgeConn vToNodeConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeSchema(WeaverVertexSchema pFromNode, string pName, string pDbName, 
									string pRelType, WeaverVertexSchema pToNode) : base(pName, pDbName) {
			FromNode = pFromNode;
			ToNode = pToNode;
			EdgeType = pRelType;

			FromNodeConn = WeaverEdgeConn.OutToZeroOrMore;
			ToNodeConn = WeaverEdgeConn.InFromZeroOrMore;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeConn FromNodeConn {
			get {
				return vFromNodeConn;
			}
			set {
				vFromNodeConn = value;

				if ( !WeaverEdge.IsConnOutgoing(vFromNodeConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an incoming "+
						"FromNodeConn value.");
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeConn ToNodeConn {
			get {
				return vToNodeConn;
			}
			set {
				vToNodeConn = value;

				if ( WeaverEdge.IsConnOutgoing(vToNodeConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an outgoing "+
						"ToNodeConn value.");
				}
			}
		}

	}

}