using Weaver.Core.Elements;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepBack<T> : WeaverStep where T : IWeaverElement {

		public IWeaverStepAs<T> BackToItem { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepBack(IWeaverStepAs<T> pBackToItem) {
			int i = pBackToItem.PathIndex;

			if ( i < 0 ) {
				throw new WeaverStepException(this,
					"The specified return item is not present in the current path.");
			}

			BackToItem = pBackToItem;
			vLabel = pBackToItem.Label;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get { return vLabel+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			//TODO: WeaverStepBack: use query parameter
			return "back('"+vLabel+"')";
		}

	}

}