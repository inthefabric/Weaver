namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverFunc<out TItem> : IWeaverItem where TItem : IWeaverItem {

		TItem CallingItem { get; }

	}

}