using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncAs<TItem> : WeaverFunc, IWeaverFuncAs<TItem>
																where TItem : IWeaverItemIndexable {

		public TItem Item { get; private set; }

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncAs(TItem pItem) {
			Item = pItem;
			vLabel = "step"+pItem.Path.Length;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get { return vLabel+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "as('"+vLabel+"')";
		}

	}

}