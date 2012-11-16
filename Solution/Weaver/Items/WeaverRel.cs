using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver.Items {
	
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

		public long Id { get; set; }
		public IWeaverRelType RelType { get; private set; }
		public bool IsFromManyNodes { get; private set; }
		public bool IsToManyNodes { get; private set; }
		public bool IsOutgoing { get; private set; }

		private WeaverRelConn? vConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel() {
			Id = -1;
			RelType = (typeof(TType) as IWeaverRelType);
		}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel(WeaverRelConn pConnection) : this() {
			Connection = pConnection;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverRelConn Connection {
			get {
				if ( vConn == null ) {
					throw new WeaverRelException(this, "Connection has not been set.");
				}

				return (WeaverRelConn)vConn;
			}
			set {
				if ( vConn != null ) {
					throw new WeaverRelException(this, "Connection has already been set.");
				}

				vConn = value;
				IsFromManyNodes = (vConn == WeaverRelConn.InFromManyNodes);
				IsToManyNodes = (vConn == WeaverRelConn.OutToManyNodes);
				IsOutgoing = (vConn == WeaverRelConn.OutToOneNode || vConn==WeaverRelConn.OutToManyNodes);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TQueryFrom FromNode {
			get {
				return new TFrom {
					Query = Query,
					IsFromNode = true,
					ExpectOneNode = !IsFromManyNodes
				};
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TQueryTo ToNode {
			get {
				return new TTo {
					Query = Query,
					IsFromNode = false,
					ExpectOneNode = !IsToManyNodes
				};
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return typeof(TFrom).Name+typeof(TType).Name+typeof(TTo).Name; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier { get { return Label+"(Id="+Id+")"; } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				return (IsOutgoing ? "out" : "in")+"E('"+Label+"')"+
					(!IsFromManyNodes && !IsToManyNodes ? "[0]" : "");
			}
		}

	}

}