using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncAs<TItem> : WeaverFunc<TItem> where TItem : IWeaverItem {

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncAs(IWeaverItem pCallingItem, string pLabel) : base(pCallingItem) {
			vLabel = pLabel;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string Label {
			get { return vLabel+""; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get { return "as('"+vLabel+"')"; }
		}

	}

}