using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {
	
	/*================================================================================================*/
	public abstract class WeaverFuncKeyIndex : WeaverFunc {

		public object Value { get; protected set; }
		public bool SingleResult { get; protected set; }

		public abstract string IndexName { get; }

	}

	/*================================================================================================*/
	public class WeaverFuncKeyIndex<T> : WeaverFuncKeyIndex where T : IWeaverItemIndexable {

		private readonly Expression<Func<T, object>> vPropFunc;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncKeyIndex(Expression<Func<T, object>> pPropFunc, object pValue,
																				bool pSingleResult) {
			vPropFunc = pPropFunc;
			Value = pValue;
			SingleResult = pSingleResult;
		}


		/*--------------------------------------------------------------------------------------------*/
		public override string IndexName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverUtil.GetPropertyName(this, vPropFunc);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var qvVal = new WeaverQueryVal(Value);
			return "V('"+IndexName+"',"+Path.Query.AddParam(qvVal)+")"+
				(SingleResult ? "[0]" : "");
		}

	}

}