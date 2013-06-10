namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public abstract class WeaverVertex<T> : WeaverElement<T>, IWeaverVertex<T>
																		where T : class, IWeaverVertex {

		public virtual bool IsFromNode { get; set; }
		public virtual bool ExpectOneNode { get; set; }
		public virtual bool IsRoot { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected TEdge NewEdge<TEdge>(WeaverEdgeConn pConn) where TEdge : IWeaverEdge, new() {
			var rel = new TEdge { Connection = pConn };
			Path.AddItem(rel);
			return rel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier {
			get { return GetType().Name+"(Id="+Id+")"; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			if ( IsRoot ) {
				return "v(0)";
			}

			return (IsFromNode ? "outV" : "inV");
		}

	}

}