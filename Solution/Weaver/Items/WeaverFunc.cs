using Fabric.Domain.Graph.Interfaces;

namespace Fabric.Domain.Graph.Items {

	/*================================================================================================*/
	public abstract class WeaverFunc<TItem> : WeaverItem, IWeaverFunc<TItem>
																		where TItem : IWeaverItem {

		public TItem CallingItem { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverFunc(IWeaverItem pCallingItem) {
			CallingItem = (TItem)pCallingItem;
		}

	}

}