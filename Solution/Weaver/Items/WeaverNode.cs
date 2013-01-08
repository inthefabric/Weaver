using Weaver.Interfaces;

namespace Weaver.Items {

	/*================================================================================================*/
	public abstract class WeaverNode : WeaverItem, IWeaverNode {

		[WeaverItemProperty]
		public long Id { get; set; }

		public virtual bool IsFromNode { get; set; }
		public virtual bool ExpectOneNode { get; set; }
		public virtual bool IsRoot { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverNode() {
			Id = -1;
		}

		/*--------------------------------------------------------------------------------------------*/
		protected TRel NewRel<TRel>(WeaverRelConn pConn) where TRel : IWeaverRel, new() {
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