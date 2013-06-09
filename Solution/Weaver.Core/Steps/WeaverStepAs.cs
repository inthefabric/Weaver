using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepAs<TItem> : WeaverStep, IWeaverStepAs<TItem>
																where TItem : IWeaverElement {

		public string Label { get; set; }
		public TItem Item { get; private set; }
		public Type ItemType { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepAs(TItem pItem) {
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