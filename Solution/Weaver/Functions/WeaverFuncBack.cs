using Fabric.Domain.Graph.Interfaces;
using Fabric.Domain.Graph.Items;

namespace Fabric.Domain.Graph.Functions {

	/*================================================================================================*/
	public class WeaverFuncBack<TToItem> : WeaverFunc<TToItem> where TToItem : IWeaverItem {

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