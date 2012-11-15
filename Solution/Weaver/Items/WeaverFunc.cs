using Weaver.Interfaces;

namespace Weaver.Items {

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