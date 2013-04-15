using System;
using System.Linq.Expressions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverUpdate<T> where T : IWeaverItemIndexable {

		public string PropName { get; private set; }
		public WeaverQueryVal PropValue { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverUpdate(IWeaverConfig pConfig, T pNode, Expression<Func<T, object>> pItemProperty) {
			PropName = pConfig.GetPropertyName(pItemProperty);
			PropValue = new WeaverQueryVal(pItemProperty.Compile()(pNode));
		}

	}

}