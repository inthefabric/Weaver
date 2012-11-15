using Fabric.Domain.Graph.Interfaces;

namespace Fabric.Domain.Graph.Items {
	
	/*================================================================================================*/
	public enum WeaverRelConn {
		OutToOneNode = 1,
		OutToManyNodes,
		InFromOneNode,
		InFromManyNodes
	}

	/*================================================================================================*/
	public abstract class WeaverRel<TQueryFrom, TFrom, TType, TQueryTo, TTo> :
														WeaverItem, IWeaverRel<TQueryFrom, TQueryTo>
															where TQueryFrom : IWeaverQueryNode
															where TFrom : TQueryFrom, IWeaverNode, new()
															where TType : IWeaverRelType
															where TQueryTo : IWeaverQueryNode
															where TTo : TQueryTo, IWeaverNode, new() {

		public WeaverRelConn Connection { get; private set; }
		public IWeaverRelType RelType { get; private set; }
		public bool FromManyNodes { get; private set; }
		public bool ToManyNodes { get; private set; }
		public bool Outgoing { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel(WeaverRelConn pConnection) {
			RelType = (typeof(TType) as IWeaverRelType);

			Connection = pConnection;
			FromManyNodes = (Connection == WeaverRelConn.InFromManyNodes);
			ToManyNodes = (Connection == WeaverRelConn.OutToManyNodes);
			Outgoing = (Connection == WeaverRelConn.OutToOneNode ||
				Connection == WeaverRelConn.OutToManyNodes);
		}

		/*--------------------------------------------------------------------------------------------*/
		public TQueryFrom FromNode {
			get { 
				return new TFrom {
					Query = Query,
					IsFromNode = true,
					ExpectOneNode = !FromManyNodes
				};
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TQueryTo ToNode {
			get { 
				return new TTo {
					Query = Query,
					IsFromNode = false,
					ExpectOneNode = !ToManyNodes
				};
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public abstract string Label { get; }

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				return (Outgoing ? "out" : "in")+"E('"+Label+"')"+
					(!FromManyNodes && !ToManyNodes ? "[0]" : "");
			}
		}

	}

}