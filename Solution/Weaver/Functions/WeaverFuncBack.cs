using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncBack<TToItem> : WeaverFunc<TToItem> where TToItem : IWeaverQueryItem {

		private readonly string vLabel;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncBack(IWeaverItem pCallingItem, string pLabel) : base(pCallingItem) {
			vLabel = pLabel;
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