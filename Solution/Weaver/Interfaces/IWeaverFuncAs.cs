using System;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverFuncAs : IWeaverFunc {

		string Label { get; }
		Type ItemType { get; }

	}
	
	/*================================================================================================*/
	public interface IWeaverFuncAs<out TItem> : IWeaverFuncAs where TItem : IWeaverItemIndexable {

		TItem Item { get; }

	}

}