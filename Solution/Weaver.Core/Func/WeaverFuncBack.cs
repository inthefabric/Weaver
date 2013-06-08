using Weaver.Core.Exceptions;
using Weaver.Core.Items;

namespace Weaver.Core.Func {

	/*================================================================================================*/
	public class WeaverFuncBack<TBack> : WeaverFunc where TBack : IWeaverItemIndexable {

		public IWeaverFuncAs<TBack> BackToItem { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncBack(IWeaverFuncAs<TBack> pBackToItem) {
			int i = pBackToItem.PathIndex;

			if ( i < 0 ) {
				throw new WeaverFuncException(this,
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