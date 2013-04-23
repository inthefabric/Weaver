using System;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncAs<TItem> : WeaverFunc, IWeaverFuncAs<TItem>
																where TItem : IWeaverItemIndexable {

		public string Label { get; set; }
		public TItem Item { get; private set; }
		public Type ItemType { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncAs(TItem pItem) {
			Item = pItem;
			ItemType = typeof(TItem);
			Label = "step"+pItem.Path.Length;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "as('"+Label+"')";
		}

	}

}