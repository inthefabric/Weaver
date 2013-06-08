using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Items;
using Weaver.Core.Query;

namespace Weaver.Core.Func {

	/*================================================================================================*/
	public enum WeaverFuncHasMode {
		Has = 1,
		HasNot
	};


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

	/*================================================================================================*/
	public class WeaverFuncHas<TItem> : WeaverFunc where TItem : IWeaverItemIndexable {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;

		public WeaverFuncHasMode Mode { get; private set; }
		public WeaverFuncHasOp? Operation { get; private set; }
		public object Value { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncHas(Expression<Func<TItem, object>> pItemProperty, WeaverFuncHasMode pMode,
												WeaverFuncHasOp? pOperation=null, object pValue=null) {
			vProp = pItemProperty;
			Mode = pMode;
			Operation = pOperation;
			Value = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = Path.Config.GetPropertyDbName(this, vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var qv = new WeaverQueryVal(Value);
			string func = "has"+(Mode == WeaverFuncHasMode.Has ? "" : "Not");

			if ( Operation == null ) {
				return func+"('"+PropertyName+"')";
			}

			string op = WeaverFuncHas.GremlinOpMap[(WeaverFuncHasOp)Operation];
			return func+"('"+PropertyName+"',Tokens.T."+op+","+Path.Query.AddParam(qv)+")";
		}

	}

}