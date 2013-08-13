using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public class WeaverStatementSetProperty<T> : WeaverStatement<T> where T : IWeaverElement {

		private readonly Expression<Func<T, object>> vProp;
		private readonly object vValue;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStatementSetProperty(Expression<Func<T, object>> pProperty, object pValue) {
			vProp = pProperty;
			vValue = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString(IWeaverPath pPath) {
			//TODO: WeaverStatementSetProperty: use query parameter
			var propName = WrapException(() => pPath.Config.GetPropertyDbName(vProp));
			var valParam = pPath.Query.AddParam(new WeaverQueryVal(vValue));
			return "it.setProperty('"+propName+"',"+valParam+")";
		}

	}

}