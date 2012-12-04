using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverUpdates<T> where T : IWeaverItem {

		private readonly List<WeaverUpdate<T>> vUpdates;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverUpdates() {
			vUpdates = new List<WeaverUpdate<T>>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddUpdate(T pNode, Expression<Func<T,object>> pFunc) {
			var u = new WeaverUpdate<T> { PropFunc = pFunc, Node = pNode };
			u.BuildStrings();
			vUpdates.Add(u);
		}

		/*--------------------------------------------------------------------------------------------*/
		public int Count { get { return vUpdates.Count; } }

		/*--------------------------------------------------------------------------------------------*/
		public KeyValuePair<string, WeaverQueryVal> this[int pIndex] {
			get {
				if ( pIndex < 0 || pIndex >= vUpdates.Count ) {
					throw new WeaverException(
						"Index "+pIndex+" is out of bounds: [0,"+vUpdates.Count+"].");
				}

				WeaverUpdate<T> u = vUpdates[pIndex];
				return new KeyValuePair<string, WeaverQueryVal>(u.PropName, u.PropVal);
			}
		}

	}


	/*================================================================================================*/
	public class WeaverUpdate<T> where T : IWeaverItem {

		public Expression<Func<T,object>> PropFunc { get; set; }
		public T Node { get; set; }
		public string PropName { get; private set; }
		public WeaverQueryVal PropVal { get; private set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void BuildStrings() {
			PropName = WeaverFuncProp.GetPropertyName(PropFunc);
			PropVal = new WeaverQueryVal(PropFunc.Compile()(Node));
		}

	}

}