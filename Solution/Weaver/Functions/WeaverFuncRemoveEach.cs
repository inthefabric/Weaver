using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncRemoveEach<TItem> : WeaverFunc where TItem : IWeaverItemIndexable {

		public bool RemoveNode { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncRemoveEach() {
			RemoveNode = (typeof(IWeaverNode).IsAssignableFrom(typeof(TItem)));
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "each{g.remove"+(RemoveNode ? "Vertex" : "Edge")+"(it)}";
		}

	}

}