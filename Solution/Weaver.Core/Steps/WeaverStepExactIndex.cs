using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;

namespace Weaver.Core.Steps {
	
	/*================================================================================================*/
	public abstract class WeaverStepExactIndex : WeaverStep {

		public object Value { get; protected set; }
		public abstract string IndexName { get; }

	}


	/*================================================================================================*/
	public class WeaverStepExactIndex<T> : WeaverStepExactIndex where T : IWeaverElement {

		private readonly Expression<Func<T, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepExactIndex(Expression<Func<T, object>> pProp, object pValue) {
			vProp = pProp;
			Value = pValue;
			SkipDotPrefix = true;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string IndexName {
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
			var qvVal = new WeaverQueryVal(Value);
			return "('"+IndexName+"',"+Path.Query.AddParam(qvVal)+")";
		}

	}

}