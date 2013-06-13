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

		private const string Compare = "com.tinkerpop.blueprints.Query.Compare.";
		private const string Text = "com.thinkaurelius.titan.core.attribute.Text.";

		public const string EqualToScript = Compare+"EQUAL";
		public const string NotEqualToScript = Compare+"NOT_EQUAL";
		public const string GreaterThanScript = Compare+"GREATER_THAN";
		public const string GreaterThanOrEqualToScript = Compare+"GREATER_THAN_EQUAL";
		public const string LessThanScript = Compare+"LESS_THAN";
		public const string LessThanOrEqualToScript = Compare+"LESS_THAN_EQUAL";

		public const string ContainsScript = Text+"CONTAINS";
		public const string PrefixScript = Text+"PREFIX";


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetOperationScript(WeaverParamElasticOp pOperation) {
			if ( OpMap == null ) {
				OpMap = new Dictionary<WeaverParamElasticOp, string>();

				OpMap.Add(WeaverParamElasticOp.EqualTo, EqualToScript);
				OpMap.Add(WeaverParamElasticOp.NotEqualTo, NotEqualToScript);
				OpMap.Add(WeaverParamElasticOp.GreaterThan, GreaterThanScript);
				OpMap.Add(WeaverParamElasticOp.GreaterThanOrEqualTo, GreaterThanOrEqualToScript);
				OpMap.Add(WeaverParamElasticOp.LessThan, LessThanScript);
				OpMap.Add(WeaverParamElasticOp.LessThanOrEqualTo, LessThanOrEqualToScript);

				OpMap.Add(WeaverParamElasticOp.Contains, ContainsScript);
				OpMap.Add(WeaverParamElasticOp.Prefix, PrefixScript);
			}

			return OpMap[pOperation];
		}

	}


	/*================================================================================================*/
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