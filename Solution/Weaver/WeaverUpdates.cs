using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
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
		public void AddUpdate(T pNode, Expression<Func<T,object>> pItemProperty) {
			vUpdates.Add(new WeaverUpdate<T>(pNode, pItemProperty));
		}

		/*--------------------------------------------------------------------------------------------*/
		public int Count {
			get { return vUpdates.Count; }
		}

		/*--------------------------------------------------------------------------------------------*/
		public KeyValuePair<string, WeaverQueryVal> this[int pIndex] {
			get {
				if ( pIndex < 0 || pIndex >= vUpdates.Count ) {
					throw new WeaverException(
						"Index "+pIndex+" is out of bounds: [0,"+vUpdates.Count+"].");
				}

				WeaverUpdate<T> u = vUpdates[pIndex];
				return new KeyValuePair<string, WeaverQueryVal>(u.PropName, u.PropValue);
			}
		}

	}

}