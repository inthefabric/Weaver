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
	public abstract class WeaverEdge : WeaverElement, IWeaverEdge {

		public bool IsFromManyVertices { get; private set; }
		public bool IsToManyVertices { get; private set; }
		public bool IsOutgoing { get; private set; }

		[WeaverProperty("label")]
		public string Label { get; set; }

		private WeaverEdgeConn? vConn;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge() {}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge(WeaverEdgeConn pConnection) {
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

				IsOutgoing = IsConnOutgoing((WeaverEdgeConn)vConn);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract Type OutVertexType { get; }
		public abstract Type InVertexType { get; }

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
		public abstract IWeaverEdgeType EdgeType { get; }

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier { get { return GetType().Name+"(Id='"+Id+"')"; } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return (IsOutgoing ? "out" : "in")+"E('"+Path.Config.GetEdgeDbName(this)+"')";
		}

		
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


	/*================================================================================================*/
	public abstract class WeaverEdge<TOut, TType, TIn> : WeaverEdge, IWeaverEdge<TOut, TIn>
																	where TOut : IWeaverVertex, new()
																	where TType : IWeaverEdgeType, new()
																	where TIn : IWeaverVertex, new() {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge() {}

		/*--------------------------------------------------------------------------------------------*/
		protected WeaverEdge(WeaverEdgeConn pConnection) : base(pConnection) {}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override Type OutVertexType {
			get { return typeof(TOut); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override Type InVertexType {
			get { return typeof(TIn); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override IWeaverEdgeType EdgeType {
			get { return new TType(); }
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

	}

}