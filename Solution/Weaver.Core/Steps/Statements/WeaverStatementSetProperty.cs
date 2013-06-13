using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;

namespace Weaver.Core.Steps.Statements {

	/*================================================================================================*/
	public class WeaverStatementSetProperty<T> : WeaverStatement<T> where T : IWeaverElement {

		private readonly IWeaverPath vPath;
		private readonly Expression<Func<T, object>> vProp;
		private readonly object vValue;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStatementSetProperty(IWeaverPath pPath, Expression<Func<T, object>> pProperty,
																						object pValue) {
			vPath = pPath;
			vProp = pProperty;
			vValue = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) {
					return vPropName;
				}

				vPropName = WrapException(() => vPath.Config.GetPropertyDbName(vProp));
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var valParam = vPath.Query.AddParam(new WeaverQueryVal(vValue));
			return "it.setProperty('"+PropertyName+"',"+valParam+")";
		}

	}

}