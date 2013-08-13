using System;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepAs<T> : WeaverStep, IWeaverStepAs<T> where T : IWeaverElement {

		public string Label { get; set; }
		public T Item { get; private set; }
		public Type ItemType { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepAs(T pElem) {
			Item = pElem;
			ItemType = typeof(T);
			Label = "step"+pElem.Path.Length;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			//TODO: WeaverStepAs: use query parameter
			return "as('"+Label+"')";
		}

	}

}