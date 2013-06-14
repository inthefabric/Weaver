using Weaver.Core;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Titan.Graph;

namespace Weaver.Titan {
	
	/*================================================================================================*/
	public static class WeaverTitanInstanceExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverTitanGraph TitanGraph<T>(this T pInstance) where T : IWeaverInstance {
			var path = new WeaverPath(pInstance.Config, new WeaverQuery());
			var g = new WeaverTitanGraph();
			path.AddItem(g);
			return g;
		}

	}

}