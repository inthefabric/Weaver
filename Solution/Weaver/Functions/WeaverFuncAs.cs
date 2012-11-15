using Fabric.Domain.Graph.Interfaces;
using Fabric.Domain.Graph.Items;

namespace Fabric.Domain.Graph.Functions {

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