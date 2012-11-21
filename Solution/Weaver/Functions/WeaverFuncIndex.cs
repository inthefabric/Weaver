using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {
	
	/*================================================================================================*/
	public abstract class WeaverFuncIndex : WeaverFunc {
	}

	/*================================================================================================*/
	public class WeaverFuncIndex<T> : WeaverFuncIndex where T : IWeaverNode {

		public string IndexName { get; private set; }
		public object Value { get; private set; }

		private readonly Expression<Func<T, object>> vFunc;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncIndex(string pIndexName, Expression<Func<T,object>> pFunc, object pValue) {
			IndexName = pIndexName;
			vFunc = pFunc;
			Value = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				var propName = WeaverFuncProp.GetPropertyName(vFunc);
				return "g.idx('"+IndexName+"').get('"+propName+"', "+
					WeaverQuery.QuoteValueIfString(Value)+")";
			}
		}

	}

}