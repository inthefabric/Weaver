using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepProp<T> : WeaverStep where T : IWeaverElement {

		private readonly Expression<Func<T, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepProp(Expression<Func<T, object>> pItemProperty) {
			vProp = pItemProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = WrapException(() => Path.Config.GetPropertyDbName(vProp));
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			if ( PropertyName == "id" ) {
				return "id";
			}

			//TODO: WeaverStepProp: use query parameter
			return "property('"+PropertyName+"')";
		}

	}

}