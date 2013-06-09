using Weaver.Core.Elements;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepBack<TBack> : WeaverStep where TBack : IWeaverElement {

		public IWeaverStepAs<TBack> BackToItem { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepBack(IWeaverStepAs<TBack> pBackToItem) {
			int i = pBackToItem.PathIndex;

			if ( i < 0 ) {
				throw new WeaverStepException(this,
					"The specified return item is not present in the current path.");
			}

			BackToItem = pBackToItem;
			vLabel = "step"+i;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get { return vLabel+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "back('"+vLabel+"')";
		}

	}

}