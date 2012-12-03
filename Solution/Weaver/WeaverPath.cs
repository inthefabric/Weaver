using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverPath<TBase> : IWeaverPath, IWeaverPath<TBase>
															where TBase : class, IWeaverItem, new() {

		public TBase BaseNode { get; private set; }
		public WeaverFuncIndex BaseIndex { get; private set; }
		private readonly IList<IWeaverItem> vItems;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(TBase pBaseNode) {
			BaseNode = pBaseNode;
			vItems = new List<IWeaverItem>();
			AddItem(BaseNode);
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath() {
			vItems = new List<IWeaverItem>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(IWeaverItem pItem) {
			vItems.Add(pItem);
			pItem.Path = this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void StartAtIndex<T>(string pIndexName, Expression<Func<T, object>> pFunc, 
																object pValue) where T : IWeaverNode {
			if ( BaseNode != null ) {
				throw new WeaverPathException(this,
					"Cannot use StartAtIndex<T>(): the BaseNode is already set.");
			}

			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncIndex<T>(pIndexName, pFunc, pValue);
			AddItem(BaseIndex);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int Length { get { return vItems.Count; } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverItem ItemAtIndex(int pIndex) {
			if ( pIndex < 0 || pIndex >= vItems.Count ) { return null; }
			return vItems[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true) {
			if ( pIndex < 0 || pIndex >= vItems.Count ) { return null; }
			var path = new List<IWeaverItem>();
			pIndex -= (pInclusive ? 0 : 1);

			for ( int i = 0 ; i <= pIndex ; ++i ) {
				if ( i == pIndex && !pInclusive ) { break; }
				path.Add(vItems[i]);
			}

			return path;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true) {
			if ( pIndex < 0 || pIndex >= vItems.Count ) { return null; }
			var path = new List<IWeaverItem>();
			var n = vItems.Count;
			pIndex += (pInclusive ? 0 : 1);

			for ( int i = pIndex ; i < n ; ++i ) {
				path.Add(vItems[i]);
			}

			return path;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int IndexOfItem(IWeaverItem pItem) {
			return vItems.IndexOf(pItem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public TItem FindAsNode<TItem>(string pLabel) where TItem : IWeaverItem {
			var n = vItems.Count;

			for ( int i = 1 ; i < n ; ++i ) {
				IWeaverItem item = vItems[i];
				WeaverFuncAs<TItem> funcAs = (item as WeaverFuncAs<TItem>);
				
				if ( funcAs == null || funcAs.Label != pLabel ) { continue; }

				IWeaverItem prev = vItems[i-1];
				if ( prev is TItem ) { return (TItem)prev; }

				throw new WeaverPathException(this, "The 'As' marker with label '"+pLabel
					+"' uses type "+prev.GetType().Name+", but type "+
					typeof(TItem).Name+" was expected.");
			}

			return default(TItem);
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetGremlinCode<T>(WeaverPath<T> pPath) where T : class, IWeaverItem, new(){
			return GetGremlinCode(pPath.vItems, (pPath.BaseIndex != null));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetGremlinCode(IList<IWeaverItem> pPathItems, bool pStartAtIndex=false) {
			string gremlin = (pStartAtIndex ? "" : "g.");

			foreach ( IWeaverItem q in pPathItems ) {
				gremlin += q.GremlinCode+'.';
			}

			return gremlin.Substring(0, gremlin.Length-1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GremlinCode { get { return GetGremlinCode(this); } }

	}

}