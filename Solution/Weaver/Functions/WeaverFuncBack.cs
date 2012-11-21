using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncBack<TBack> : WeaverFunc where TBack : IWeaverItem {

		public TBack BackToItem { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncBack(IWeaverPath pPath, TBack pBackToItem) {
			int i = pPath.IndexOfItem(pBackToItem);

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
		public override string GremlinCode {
			get { return "back('"+vLabel+"')"; }
		}

	}

}