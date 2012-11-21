using Weaver.Interfaces;

namespace Weaver.Items {

	/*================================================================================================*/
	public abstract class WeaverNode : WeaverItem, IWeaverNode {

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
		public TRel NewRel<TRel>(WeaverRelConn pConn) where TRel : IWeaverRel, new() {
			var rel = new TRel { Connection = pConn };
			Path.AddItem(rel);
			return rel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string ItemIdentifier { get { return this+"(Id="+Id+")"; } }

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				return (IsRoot ? "v(0)" :
					(IsFromNode ? "outV" : "inV")+(ExpectOneNode ? "(0)" : "")
				);
			}
		}

	}

}