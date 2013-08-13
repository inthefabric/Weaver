using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public class WeaverStatementRemoveProperty<T> : WeaverStatement<T> where T : IWeaverElement {

		private readonly Expression<Func<T, object>> vProp;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStatementRemoveProperty(Expression<Func<T, object>> pProperty) {
			vProp = pProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString(IWeaverPath pPath) {
			//TODO: WeaverStatementRemoveProperty: use query parameter
			var propName = WrapException(() => pPath.Config.GetPropertyDbName(vProp));
			return "it.removeProperty('"+propName+"')";
		}

	}

}