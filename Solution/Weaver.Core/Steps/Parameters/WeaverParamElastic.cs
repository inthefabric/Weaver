using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps.Parameters {

	/*================================================================================================*/
	public enum WeaverParamElasticOp {

		//Query.Compare (Blueprints)
		EqualTo = 1,
		NotEqualTo,
		GreaterThan,
		GreaterThanOrEqualTo,
		LessThan,
		LessThanOrEqualTo,

		//Text (Titan)
		Contains,
		Prefix
	};

	
	/*================================================================================================*/
	public static class WeaverParamElastic {

		internal static IDictionary<WeaverParamElasticOp, string> OpMap;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetOperationScript(WeaverParamElasticOp pOperation) {
			if ( OpMap == null ) {
				const string comp = "com.tinkerpop.blueprints.Query.Compare.";
				const string text = "com.thinkaurelius.titan.core.attribute.Text.";

				OpMap = new Dictionary<WeaverParamElasticOp, string>();

				OpMap.Add(WeaverParamElasticOp.EqualTo, comp+"EQUAL");
				OpMap.Add(WeaverParamElasticOp.NotEqualTo, comp+"NOT_EQUAL");
				OpMap.Add(WeaverParamElasticOp.GreaterThan, comp+"GREATER_THAN");
				OpMap.Add(WeaverParamElasticOp.GreaterThanOrEqualTo, comp+"GREATER_THAN_EQUAL");
				OpMap.Add(WeaverParamElasticOp.LessThan, comp+"LESS_THAN");
				OpMap.Add(WeaverParamElasticOp.LessThanOrEqualTo, comp+"LESS_THAN_EQUAL");

				OpMap.Add(WeaverParamElasticOp.Contains, text+"CONTAINS");
				OpMap.Add(WeaverParamElasticOp.Prefix, text+"PREFIX");
			}

			return OpMap[pOperation];
		}

	}


	/*================================================================================================*/
	//TEST: Core.Steps.Parameters.WeaverParamElastic
	public class WeaverParamElastic<T> : WeaverParam<T>, IWeaverParamElastic<T> 
																			where T : IWeaverElement {

		public Expression<Func<T, object>> Property { get; private set; }
		public WeaverParamElasticOp Operation { get; private set; }
		public object Value { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverParamElastic(Expression<Func<T, object>> pProperty,
													WeaverParamElasticOp pOperation, object pValue) {
			Property = pProperty;
			Operation = pOperation;
			Value = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetOperationScript() {
			return WeaverParamElastic.GetOperationScript(Operation);
		}

	}

}