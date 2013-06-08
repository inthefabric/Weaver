using System;
using Weaver.Core.Items;

namespace Weaver.Core.Func {

	/*================================================================================================*/
	public interface IWeaverFuncAs : IWeaverFunc {

		string Label { get; set; }
		Type ItemType { get; }

	}
	
	/*================================================================================================*/
	public interface IWeaverFuncAs<out TItem> : IWeaverFuncAs where TItem : IWeaverItemIndexable {

		TItem Item { get; }

	}

}