using System;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Elements {
	
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
	public abstract class WeaverEdge<TEdge, TFrom, TType, TTo> : WeaverElement<TEdge>,
																	IWeaverEdge<TEdge, TFrom, TTo>
																	where TEdge : class, IWeaverEdge
																	where TFrom : IWeaverVertex, new()
																	where TType : IWeaverEdgeType, new()
																	where TTo : IWeaverVertex, new() {

		public bool IsFromManyVertices { get; private set; }
		public bool IsToManyVertices { get; private set; }
		public bool IsOutgoing { get; private set; }

		private WeaverEdgeConn? vConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge() {}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge(WeaverEdgeConn pConnection) : this() {
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
				
				IsFromManyVertices = (
					vConn == WeaverEdgeConn.InFromZeroOrMore || 
					vConn == WeaverEdgeConn.InFromOneOrMore
				);
				
				IsToManyVertices = (
					vConn == WeaverEdgeConn.OutToZeroOrMore ||
					vConn == WeaverEdgeConn.OutToOneOrMore
				);
				
				IsOutgoing = WeaverEdge.IsConnOutgoing((WeaverEdgeConn)vConn);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TFrom FromVertex {
			get {
				var n = new TFrom { IsFromVertex = true, ExpectOneVertex = !IsFromManyVertices };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TTo ToVertex {
			get {
				var n = new TTo { IsFromVertex = false, ExpectOneVertex = !IsToManyVertices };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type FromVertexType { get { return typeof(TFrom); } }
		public Type ToVertexType { get { return typeof(TTo); } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverEdgeType EdgeType {
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