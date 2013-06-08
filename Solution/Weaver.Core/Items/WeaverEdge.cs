using System;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Items {
	
	/*================================================================================================*/
	public enum WeaverEdgeConn {

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
	public abstract class WeaverRel<TFrom, TType, TTo> : WeaverItem, IWeaverRel<TFrom, TTo>
																	where TFrom : IWeaverVertex, new()
																	where TType : IWeaverEdgeType, new()
																	where TTo : IWeaverVertex, new() {

		public string Id { get; set; }
		public bool IsFromManyNodes { get; private set; }
		public bool IsToManyNodes { get; private set; }
		public bool IsOutgoing { get; private set; }

		private WeaverEdgeConn? vConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel() {}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverRel(WeaverEdgeConn pConnection) : this() {
			Connection = pConnection;
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverEdgeConn Connection {
			get {
				if ( vConn == null ) {
					throw new WeaverEdgeException(this, "Connection has not been set.");
				}

				return (WeaverEdgeConn)vConn;
			}
			set {
				if ( vConn != null ) {
					throw new WeaverEdgeException(this, "Connection has already been set.");
				}

				vConn = value;
				
				IsFromManyNodes = (
					vConn == WeaverEdgeConn.InFromZeroOrMore || 
					vConn == WeaverEdgeConn.InFromOneOrMore
				);
				
				IsToManyNodes = (
					vConn == WeaverEdgeConn.OutToZeroOrMore ||
					vConn == WeaverEdgeConn.OutToOneOrMore
				);
				
				IsOutgoing = WeaverEdge.IsConnOutgoing((WeaverEdgeConn)vConn);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TFrom FromNode {
			get {
				var n = new TFrom { IsFromNode = true, ExpectOneNode = !IsFromManyNodes };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TTo ToNode {
			get {
				var n = new TTo { IsFromNode = false, ExpectOneNode = !IsToManyNodes };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type FromNodeType { get { return typeof(TFrom); } }
		public Type ToNodeType { get { return typeof(TTo); } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverEdgeType RelType {
			get {
				return new TType();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return typeof(TFrom).Name+typeof(TType).Name+typeof(TTo).Name; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier { get { return Label+"(Id='"+Id+"')"; } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (IsOutgoing ? "out" : "in")+"E('"+Path.Config.GetItemDbName(Label)+"')";
		}

	}

	/*================================================================================================*/
	public static class WeaverEdge {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static bool IsConnOutgoing(WeaverEdgeConn pConn) {
			return (
				pConn == WeaverEdgeConn.OutToOne || 
				pConn == WeaverEdgeConn.OutToOneOrMore ||
				pConn == WeaverEdgeConn.OutToZeroOrMore ||
				pConn == WeaverEdgeConn.OutToZeroOrOne
			);
		}

	}

}