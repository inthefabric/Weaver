using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncProp<TItem> : WeaverFunc, IWeaverItemWithPath where TItem : IWeaverItemIndexable {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncProp(Expression<Func<TItem, object>> pItemProperty) {
			vProp = pItemProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {

				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverUtil.GetPropertyName(this, vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return PropertyName;
		}

	}

}