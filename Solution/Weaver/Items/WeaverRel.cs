using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver.Items {
	
	/*================================================================================================*/
	public enum WeaverRelConn {

		OutToOne = 1,
		OutToOneOrMore,
		OutToZeroOrMore,
		OutToZeroOrOne,

		InFromOne,
		InFromOneOrMore,
		InFromZeroOrMore,
		InFromZeroOrOne

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
				
				IsFromManyNodes = (
					vConn == WeaverRelConn.InFromZeroOrMore || 
					vConn == WeaverRelConn.InFromOneOrMore
				);
				
				IsToManyNodes = (
					vConn == WeaverRelConn.OutToZeroOrMore ||
					vConn == WeaverRelConn.OutToOneOrMore
				);
				
				IsOutgoing = WeaverRel.IsConnOutgoing((WeaverRelConn)vConn);
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

	/*================================================================================================*/
	public static class WeaverRel {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsConnOutgoing(WeaverRelConn pConn) {
			return (
				pConn == WeaverRelConn.OutToOne || 
				pConn == WeaverRelConn.OutToOneOrMore ||
				pConn == WeaverRelConn.OutToZeroOrMore ||
				pConn == WeaverRelConn.OutToZeroOrOne
			);
		}

	}

}