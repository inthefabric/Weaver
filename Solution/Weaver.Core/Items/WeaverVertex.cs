namespace Weaver.Core.Items {

	/*================================================================================================*/
	public abstract class WeaverVertex : WeaverItem, IWeaverVertex {

		[WeaverItemProperty]
		public string Id { get; set; }

		public virtual bool IsFromNode { get; set; }
		public virtual bool ExpectOneNode { get; set; }
		public virtual bool IsRoot { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverVertex() {}

		/*--------------------------------------------------------------------------------------------*/
		protected TRel NewRel<TRel>(WeaverEdgeConn pConn) where TRel : IWeaverEdge, new() {
			var rel = new TRel { Connection = pConn };
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