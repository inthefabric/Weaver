using System;
using System.Linq.Expressions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public static class WeaverNodeExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T ToNodeVar<T>(this T pCallingItem, IWeaverVarAlias<T> pNodeVar)
																				where T : IWeaverNode {
			var func = new WeaverFuncToNodeVar<T>(pNodeVar);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

	}

}