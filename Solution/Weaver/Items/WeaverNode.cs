using Weaver.Interfaces;

namespace Weaver.Items {

	/*================================================================================================*/
	public abstract class WeaverNode : WeaverItem, IWeaverNode {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual bool IsFromNode { get; set; }
		public virtual bool ExpectOneNode { get; set; }
		public virtual bool IsRoot { get; protected set; }

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