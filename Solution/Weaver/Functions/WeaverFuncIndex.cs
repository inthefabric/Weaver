using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {
	
	/*================================================================================================*/
	public abstract class WeaverFuncIndex : WeaverFunc {

		public string IndexName { get; protected set; }
		public object Value { get; protected set; }

		public abstract string PropertyName { get; }

	}

	/*================================================================================================*/
	public class WeaverFuncIndex<T> : WeaverFuncIndex where T : IWeaverNode {

		private readonly Expression<Func<T, object>> vFunc;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncIndex(string pIndexName, Expression<Func<T,object>> pFunc, object pValue) {
			IndexName = pIndexName;
			vFunc = pFunc;
			Value = pValue;
		}


		/*--------------------------------------------------------------------------------------------*/
		public override string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverFuncProp.GetPropertyName(vFunc);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				return "g.idx("+IndexName+").get('"+PropertyName+"', "+
					WeaverQuery.QuoteValueIfString(Value)+")";
			}
		}

	}

}