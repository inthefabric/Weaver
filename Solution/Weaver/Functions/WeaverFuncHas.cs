using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public enum WeaverFuncHasOp {
		EqualTo = 1,
		NotEqualTo,
		GreaterThan,
		GreterThanOrEqualTo,
		LessThan,
		LessThanOrEqualTo
	};


	/*================================================================================================*/
	public abstract class WeaverFuncHas {

		public static Dictionary<WeaverFuncHasOp, string> GremlinOpMap = BuildOpMap();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Dictionary<WeaverFuncHasOp, string> BuildOpMap() {
			var map = new Dictionary<WeaverFuncHasOp, string>();
			map.Add(WeaverFuncHasOp.EqualTo, "eq");
			map.Add(WeaverFuncHasOp.NotEqualTo, "neq");
			map.Add(WeaverFuncHasOp.GreaterThan, "gt");
			map.Add(WeaverFuncHasOp.GreterThanOrEqualTo, "gte");
			map.Add(WeaverFuncHasOp.LessThan, "lt");
			map.Add(WeaverFuncHasOp.LessThanOrEqualTo, "lte");
			return map;
		}

	}

	//TODO: use generic type for Expression return value?

	/*================================================================================================*/
	public class WeaverFuncHas<TItem> : WeaverFunc<TItem>, IWeaverProp where TItem : IWeaverItem {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;

		public WeaverFuncHasOp Operation { get; private set; }
		public object Value { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncHas(IWeaverItem pCallingItem,
						Expression<Func<TItem, object>> pItemProperty, WeaverFuncHasOp pOperation,
																object pValue) : base(pCallingItem) {
			vProp = pItemProperty;
			Operation = pOperation;
			Value = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverFuncProp<TItem>.GetPropertyName(this, vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get {
				string v = (Value == null ? "null" : Value.ToString());
				if ( Value is string ) { v = "'"+v+"'"; }
				return "has('"+PropertyName+"', T."+
					WeaverFuncHas.GremlinOpMap[Operation]+", "+v+")";
			}
		}

	}

}