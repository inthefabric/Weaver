using System;
using System.Linq.Expressions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverUpdate<T> where T : IWeaverItem {

		public string PropName { get; private set; }
		public WeaverQueryVal PropValue { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverUpdate(T pNode, Expression<Func<T, object>> pItemProperty) {
			PropName = WeaverUtil.GetPropertyName(pItemProperty);
			PropValue = new WeaverQueryVal(pItemProperty.Compile()(pNode), false);
		}

	}

}