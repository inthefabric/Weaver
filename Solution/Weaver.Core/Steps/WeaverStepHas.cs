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
		GreaterThanOrEqualTo,
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
			map.Add(WeaverStepHasOp.GreaterThanOrEqualTo, "gte");
			map.Add(WeaverStepHasOp.LessThan, "lt");
			map.Add(WeaverStepHasOp.LessThanOrEqualTo, "lte");
			return map;
		}

	}

	/*================================================================================================*/
	public class WeaverStepHas<T> : WeaverStep where T : IWeaverElement {

		private readonly Expression<Func<T, object>> vProp;
		private string vPropName;

		public WeaverStepHasMode Mode { get; private set; }
		public WeaverStepHasOp? Operation { get; private set; }
		public object Value { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepHas(Expression<Func<T, object>> pProperty, WeaverStepHasMode pMode,
												WeaverStepHasOp? pOperation=null, object pValue=null) {
			vProp = pProperty;
			Mode = pMode;
			Operation = pOperation;
			Value = pValue;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {
				if ( vPropName != null ) {
					return vPropName;
				}

				vPropName = WrapException(() => Path.Config.GetPropertyDbName(vProp));
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			var qv = new WeaverQueryVal(Value);
			string func = "has"+(Mode == WeaverStepHasMode.Has ? "" : "Not");

			//TODO: WeaverStepHas: use query parameter (for PropertyName)

			if ( Operation == null ) {
				return func+"('"+PropertyName+"')";
			}

			string op = WeaverStepHas.GremlinOpMap[(WeaverStepHasOp)Operation];
			return func+"('"+PropertyName+"',Tokens.T."+op+","+Path.Query.AddParam(qv)+")";
		}

	}

}