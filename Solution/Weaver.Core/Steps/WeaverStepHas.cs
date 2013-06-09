using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public enum WeaverStepHasMode {
		Has = 1,
		HasNot
	};


	/*================================================================================================*/
	public enum WeaverStepHasOp {
		EqualTo = 1,
		NotEqualTo,
		GreaterThan,
		GreterThanOrEqualTo,
		LessThan,
		LessThanOrEqualTo
	};


	/*================================================================================================*/
	public abstract class WeaverStepHas {

		public static Dictionary<WeaverStepHasOp, string> GremlinOpMap = BuildOpMap();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static Dictionary<WeaverStepHasOp, string> BuildOpMap() {
			var map = new Dictionary<WeaverStepHasOp, string>();
			map.Add(WeaverStepHasOp.EqualTo, "eq");
			map.Add(WeaverStepHasOp.NotEqualTo, "neq");
			map.Add(WeaverStepHasOp.GreaterThan, "gt");
			map.Add(WeaverStepHasOp.GreterThanOrEqualTo, "gte");
			map.Add(WeaverStepHasOp.LessThan, "lt");
			map.Add(WeaverStepHasOp.LessThanOrEqualTo, "lte");
			return map;
		}

	}

	/*================================================================================================*/
	public class WeaverStepHas<TItem> : WeaverStep where TItem : IWeaverElement {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;

		public WeaverStepHasMode Mode { get; private set; }
		public WeaverStepHasOp? Operation { get; private set; }
		public object Value { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepHas(Expression<Func<TItem, object>> pItemProperty, WeaverStepHasMode pMode,
												WeaverStepHasOp? pOperation=null, object pValue=null) {
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
			string func = "has"+(Mode == WeaverStepHasMode.Has ? "" : "Not");

			if ( Operation == null ) {
				return func+"('"+PropertyName+"')";
			}

			string op = WeaverStepHas.GremlinOpMap[(WeaverStepHasOp)Operation];
			return func+"('"+PropertyName+"',Tokens.T."+op+","+Path.Query.AddParam(qv)+")";
		}

	}

}