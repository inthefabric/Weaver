﻿using System;
using System.Linq.Expressions;
using Weaver.Core.Items;
using Weaver.Core.Query;

namespace Weaver.Core.Func {
	
	/*================================================================================================*/
	public abstract class WeaverFuncExactIndex : WeaverFunc {

		public object Value { get; protected set; }
		public abstract string IndexName { get; }

	}


	/*================================================================================================*/
	public class WeaverFuncExactIndex<T> : WeaverFuncExactIndex where T : IWeaverItemIndexable {

		private readonly Expression<Func<T, object>> vPropFunc;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncExactIndex(Expression<Func<T, object>> pPropFunc, object pValue) {
			vPropFunc = pPropFunc;
			Value = pValue;
		}


		/*--------------------------------------------------------------------------------------------*/
		public override string IndexName {
			get {
				if ( vPropName != null ) { return vPropName; }
				vPropName = Path.Config.GetPropertyDbName(this, vPropFunc);
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