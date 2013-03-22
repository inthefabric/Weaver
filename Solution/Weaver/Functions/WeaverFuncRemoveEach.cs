using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncRemoveEach<TItem> : WeaverFunc, IWeaverPathEnder	
																	where TItem : IWeaverItemIndexable {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return "remove()";
		}

	}

}