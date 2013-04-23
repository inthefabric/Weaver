using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverUpdates<T> where T : IWeaverItemIndexable {

		private readonly IWeaverConfig vConfig;
		private readonly List<WeaverUpdate<T>> vUpdates;
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal WeaverUpdates(IWeaverConfig pConfig) {
			vConfig = pConfig;
			vUpdates = new List<WeaverUpdate<T>>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddUpdate(T pNode, Expression<Func<T,object>> pItemProperty) {
			vUpdates.Add(new WeaverUpdate<T>(vConfig, pNode, pItemProperty));
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
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