using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {
	
	/*================================================================================================*/
	public abstract class WeaverFuncIndex : WeaverFunc {

		public string IndexName { get; protected set; }
		public object Value { get; protected set; }
		public bool SingleResult { get; protected set; }

		public abstract string PropertyName { get; }

	}

	/*================================================================================================*/
	public class WeaverFuncIndex<T> : WeaverFuncIndex where T : IWeaverItemIndexable {

		private readonly Expression<Func<T, object>> vFunc;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncIndex(string pIndexName, Expression<Func<T,object>> pFunc, object pValue,
																				bool pSingleResult) {
			IndexName = pIndexName;
			vFunc = pFunc;
			Value = pValue;
			SingleResult = pSingleResult;
		}


		/*--------------------------------------------------------------------------------------------*/
		public override string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverUtil.GetPropertyName(this, vFunc);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var qvIdx = new WeaverQueryVal(IndexName, false);
			var qvVal = new WeaverQueryVal(Value, false);
			return "idx("+Path.Query.AddParam(qvIdx)+").get('"+PropertyName+"',"+
				Path.Query.AddParamIfString(qvVal)+")"+(SingleResult ? "[0]" : "");
		}

	}

}