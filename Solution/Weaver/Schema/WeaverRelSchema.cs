using Weaver.Exceptions;
using Weaver.Items;

namespace Weaver.Schema {

	/*================================================================================================*/
	public class WeaverRelSchema : WeaverItemSchema {

		public WeaverNodeSchema FromNode { get; private set; }
		public WeaverNodeSchema ToNode { get; private set; }

		private WeaverRelConn vFromNodeConn;
		private WeaverRelConn vToNodeConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelSchema(WeaverNodeSchema pFromNode, string pName, string pDbName,
													WeaverNodeSchema pToNode) : base(pName, pDbName) {
			FromNode = pFromNode;
			ToNode = pToNode;
			FromNodeConn = WeaverRelConn.OutToZeroOrMore;
			ToNodeConn = WeaverRelConn.InFromZeroOrMore;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelConn FromNodeConn {
			get {
				return vFromNodeConn;
			}
			set {
				vFromNodeConn = value;

				if ( !WeaverRel.IsConnOutgoing(vFromNodeConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an incoming "+
						"FromNodeConn value.");
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelConn ToNodeConn {
			get {
				return vToNodeConn;
			}
			set {
				vToNodeConn = value;

				if ( WeaverRel.IsConnOutgoing(vToNodeConn) ) {
					throw new WeaverException("Item '"+Name+"' cannot use an outgoing "+
						"ToNodeConn value.");
				}
			}
		}

	}

}