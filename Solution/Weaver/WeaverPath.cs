﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public abstract class WeaverPath : IWeaverPath {

		public IWeaverQuery Query { get; private set; }

		protected readonly IList<IWeaverItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverPath(IWeaverQuery pQuery) {
			Query = pQuery;
			vItems = new List<IWeaverItem>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(IWeaverItem pItem) {
			vItems.Add(pItem);
			pItem.Path = this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string BuildParameterizedScript() {
			string s = "g";

			foreach ( IWeaverItem item in vItems ) {
				s += (s == "" ? "" : ".")+item.BuildParameterizedString();
			}

			return s;
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
		
	}


	/*================================================================================================*/
	public class WeaverPath<TBase> : WeaverPath, IWeaverPath<TBase>
															where TBase : class, IWeaverItem, new() {

		public TBase BaseNode { get; protected set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverQuery pQuery) : base(pQuery) {}

		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverQuery pQuery, TBase pBaseNode) : this(pQuery) {
			BaseNode = pBaseNode;
			AddItem(BaseNode);
		}

	}


	/*================================================================================================*/
	public class WeaverPathFromManualIndex<TBase> : WeaverPath<TBase>, IWeaverPathFromManualIndex<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		public WeaverFuncManualIndex<TBase> BaseIndex { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathFromManualIndex(IWeaverQuery pQuery, string pIndexName,
												Expression<Func<TBase, object>> pPropFunc, object pValue,
															bool pSingleResult=true) : base(pQuery) {
			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncManualIndex<TBase>(pIndexName, pPropFunc, pValue, pSingleResult);
			AddItem(BaseIndex);
		}

	}


	/*================================================================================================*/
	public class WeaverPathFromKeyIndex<TBase> : WeaverPath<TBase>, IWeaverPathFromKeyIndex<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		public WeaverFuncKeyIndex<TBase> BaseIndex { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathFromKeyIndex(IWeaverQuery pQuery, Expression<Func<TBase, object>> pPropFunc,
												object pValue, bool pSingleResult=true) : base(pQuery) {
			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncKeyIndex<TBase>(pPropFunc, pValue, pSingleResult);
			AddItem(BaseIndex);
		}

	}

}