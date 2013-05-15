using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {
	
	/*================================================================================================*/
	public abstract class WeaverFuncContainsIndex : WeaverFunc {

		public string Value { get; protected set; }

		public abstract string IndexName { get; }

	}

	/*================================================================================================*/
	public class WeaverFuncContainsIndex<T> : WeaverFuncContainsIndex where T : IWeaverItemIndexable {

		private readonly Expression<Func<T, object>> vPropFunc;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncContainsIndex(Expression<Func<T, object>> pPropFunc, string pValue) {
			vPropFunc = pPropFunc;
			Value = pValue;
		}


		/*--------------------------------------------------------------------------------------------*/
		public override string IndexName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = Path.Config.GetPropertyDbName(this, vPropFunc);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var qvVal = new WeaverQueryVal(Value);
			return "query().has('"+IndexName+"',com.thinkaurelius.titan.core.attribute.Text.CONTAINS,"+
				Path.Query.AddParam(qvVal)+")";
		}

	}

}