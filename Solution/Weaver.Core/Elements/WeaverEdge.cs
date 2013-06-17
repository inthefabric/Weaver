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
	public abstract class WeaverEdge<TEdge, TOut, TType, TIn> : WeaverElement<TEdge>,
																	IWeaverEdge<TEdge, TOut, TIn>
																	where TEdge : class, IWeaverEdge
																	where TOut : IWeaverVertex, new()
																	where TType : IWeaverEdgeType, new()
																	where TIn : IWeaverVertex, new() {

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
		public TOut OutVertex {
			get {
				var n = new TOut { IsFromVertex = true, ExpectOneVertex = !IsFromManyVertices };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public TIn InVertex {
			get {
				var n = new TIn { IsFromVertex = false, ExpectOneVertex = !IsToManyVertices };
				Path.AddItem(n);
				return n;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public Type OutVertexType { get { return typeof(TOut); } }
		public Type InVertexType { get { return typeof(TIn); } }

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsValidOutVertexType(Type pType) {
			return OutVertexType.IsAssignableFrom(pType);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsValidInVertexType(Type pType) {
			return InVertexType.IsAssignableFrom(pType);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverEdgeType EdgeType {
			get {
				return new TType();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string Label {
			get { return typeof(TOut).Name+typeof(TType).Name+typeof(TIn).Name; }
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