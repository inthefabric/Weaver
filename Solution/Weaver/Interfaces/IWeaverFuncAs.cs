namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverFuncAs<out TItem> : IWeaverFunc where TItem : IWeaverItemIndexable {

		TItem Item { get; }
		string Label { get; }

	}

}