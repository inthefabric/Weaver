using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public class WeaverPath : IWeaverPath {

		public IWeaverQuery Query { get; protected set; }
		public WeaverFuncIndex BaseIndex { get; protected set; }
		public bool Finished { get; protected set; }

		protected readonly IList<IWeaverItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal WeaverPath(WeaverQuery pQuery) {
			Query = pQuery;
			vItems = new List<IWeaverItem>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(IWeaverItem pItem) {
			vItems.Add(pItem);
			pItem.Path = this;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int Length { get { return vItems.Count; } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverItem ItemAtIndex(int pIndex) {
			ThrowIfOutOfBounds(pIndex);
			return vItems[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true) {
			pIndex -= (pInclusive ? 0 : 1);
			ThrowIfOutOfBounds(pIndex);

			var path = new List<IWeaverItem>();

			for ( int i = 0 ; i <= pIndex ; ++i ) {
				path.Add(vItems[i]);
			}

			return path;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true) {
			pIndex += (pInclusive ? 0 : 1);
			ThrowIfOutOfBounds(pIndex);

			var path = new List<IWeaverItem>();
			var n = vItems.Count;

			for ( int i = pIndex ; i < n ; ++i ) {
				path.Add(vItems[i]);
			}

			return path;
		}

		/*--------------------------------------------------------------------------------------------*/
		private void ThrowIfOutOfBounds(int pIndex) {
			if ( pIndex < 0 || pIndex >= vItems.Count ) {
				throw new WeaverPathException(this,
					"Index "+pIndex+" is out of bounds: [0,"+vItems.Count+"].");
			}
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

				throw new WeaverPathException(this, "The 'As' marker with label '"+pLabel+"' uses "+
					"type "+prev.GetType().Name+", but type "+typeof(TItem).Name+" was expected.");
			}

			return default(TItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string GetParameterizedScriptAndFinish() {
			if ( Finished ) {
				throw new WeaverException("Path has already been finished.");
			}

			Finished = true;
			string s = (BaseIndex != null ? "" : "g");

			foreach ( IWeaverItem item in vItems ) {
				s += (s == "" ? "" : ".")+item.BuildParameterizedString();
			}

			return s;
		}

	}


	/*================================================================================================*/
	public class WeaverPath<TBase> : WeaverPath, IWeaverPath<TBase>
															where TBase : class, IWeaverItem, new() {

		public TBase BaseNode { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(WeaverQuery pQuery, TBase pBaseNode) : this(pQuery) {
			BaseNode = pBaseNode;
			AddItem(BaseNode);
		}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(WeaverQuery pQuery) : base(pQuery) {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void StartWithIndex<T>(string pIndexName, Expression<Func<T, object>> pFunc, 
														object pValue) where T : TBase, IWeaverNode {
			if ( BaseNode != null ) {
				throw new WeaverPathException(this,
					"Cannot use StartAtIndex<T>(): the BaseNode is already set.");
			}

			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncIndex<T>(pIndexName, pFunc, pValue);
			AddItem(BaseIndex);
		}

	}

}