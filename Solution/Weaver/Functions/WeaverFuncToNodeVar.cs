using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncToNodeVar<TItem> : WeaverFunc where TItem : IWeaverItemIndexable {

		private readonly IWeaverVarAlias<TItem> vNodeVar;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncToNodeVar(IWeaverVarAlias<TItem> pNodeVar) {
			vNodeVar = pNodeVar;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "each{"+vNodeVar.Name+"=g.v(it)}";
		}

	}

}