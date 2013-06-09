using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepProp<TItem> : WeaverStep where TItem : IWeaverElement {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepProp(Expression<Func<TItem, object>> pItemProperty) {
			vProp = pItemProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = Path.Config.GetPropertyDbName(this, vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			if ( PropertyName.ToLower() == "id" ) {
				return "id";
			}

			return "property('"+PropertyName+"')";
		}

	}

}