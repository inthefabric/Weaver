using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public interface IWeaverStepAs : IWeaverStep {

		string Label { get; set; }
		Type ItemType { get; }

	}
	
	/*================================================================================================*/
	public interface IWeaverStepAs<out TItem> : IWeaverStepAs where TItem : IWeaverElement {

		TItem Item { get; }

	}

}