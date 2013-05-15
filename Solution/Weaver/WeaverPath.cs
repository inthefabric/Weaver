using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {
	
	/*================================================================================================*/
	public abstract class WeaverPath : IWeaverPath {

		public IWeaverConfig Config { get; private set; }
		public IWeaverQuery Query { get; private set; }

		protected readonly IList<IWeaverItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverPath(IWeaverConfig pConfig, IWeaverQuery pQuery) {
			Config = pConfig;
			Query = pQuery;
			vItems = new List<IWeaverItem>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(IWeaverItem pItem) {
			int n = vItems.Count;

			if ( n > 0 && vItems[n-1] is IWeaverPathEnder ) {
				throw new WeaverPathException(this,
					"This path was ended by the previous item ("+vItems[n-1]+").");
			}

			vItems.Add(pItem);
			pItem.Path = this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual string BuildParameterizedScript() {
			string s = "g";

			foreach ( IWeaverItem item in vItems ) {
				s += (s == "" || item.SkipDotPrefix ? "" : ".")+item.BuildParameterizedString();
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
	internal class WeaverPath<TBase> : WeaverPath, IWeaverPath<TBase>
															where TBase : class, IWeaverItem, new() {

		public TBase BaseNode { get; protected set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverConfig pConfig, IWeaverQuery pQuery) : base(pConfig, pQuery) { }

		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverConfig pConfig, IWeaverQuery pQuery, TBase pBaseNode) :
																				this(pConfig, pQuery) {
			BaseNode = pBaseNode;
			AddItem(BaseNode);
		}

	}


	/*================================================================================================*/
	internal class WeaverPathFromNodeId<TBase> : WeaverPath<TBase>, IWeaverPathFromNodeId<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		public string NodeId { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathFromNodeId(IWeaverConfig pConfig, IWeaverQuery pQuery, string pNodeId) : 
																				base(pConfig, pQuery) {
			BaseNode = new TBase { Path = this };
			NodeId = pNodeId;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedScript() {
			char type = (typeof(IWeaverRel).IsAssignableFrom(typeof(TBase)) ? 'e' : 'v');
			return "g."+type+"("+Query.AddStringParam(NodeId)+")"+
				base.BuildParameterizedScript().Substring(1);
		}

	}


	/*================================================================================================*/
	internal class WeaverPathFromVarAlias<TBase> : WeaverPath<TBase>, IWeaverPathFromVarAlias<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		public IWeaverVarAlias<TBase> BaseVar { get; private set; }
		public bool CopyItemIntoVar { get; private set; }
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathFromVarAlias(IWeaverConfig pConfig, IWeaverQuery pQuery, 
					IWeaverVarAlias<TBase> pBaseVar, bool pCopyItemIntoVar) : base(pConfig, pQuery) {
			BaseNode = new TBase { Path = this };
			BaseVar = pBaseVar;
			CopyItemIntoVar = pCopyItemIntoVar;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedScript() {
			string s;

			if ( CopyItemIntoVar ) {
				char type = (typeof(IWeaverRel).IsAssignableFrom(typeof(TBase)) ? 'e' : 'v');
				s = "g."+type+"("+BaseVar.Name+")";
			}
			else {
				s = BaseVar.Name;
			}

			return s+base.BuildParameterizedScript().Substring(1);
		}

	}


	/*================================================================================================*/
	internal class WeaverPathFromKeyIndex<TBase> : WeaverPath<TBase>, IWeaverPathFromKeyIndex<TBase>
													where TBase : class, IWeaverItemIndexable, new() {

		public WeaverFuncKeyIndex<TBase> BaseIndex { get; protected set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathFromKeyIndex(IWeaverConfig pConfig, IWeaverQuery pQuery, 
					Expression<Func<TBase, object>> pPropFunc, object pValue) : base(pConfig, pQuery) {
			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncKeyIndex<TBase>(pPropFunc, pValue);
			AddItem(BaseIndex);
		}

	}
	
	
	/*================================================================================================*/
	internal class WeaverPathWithContainsIndex<TBase> : WeaverPath<TBase>, 
													IWeaverPathWithContainsIndex<TBase>
													where TBase : class, IWeaverItemIndexable, new() {
		
		public WeaverFuncContainsIndex<TBase> BaseIndex { get; protected set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPathWithContainsIndex(IWeaverConfig pConfig, IWeaverQuery pQuery, 
			                              Expression<Func<TBase, object>> pPropFunc, string pValue) : 
			                              base(pConfig, pQuery) {
			BaseNode = new TBase { Path = this };
			BaseIndex = new WeaverFuncContainsIndex<TBase>(pPropFunc, pValue);
			AddItem(BaseIndex);
		}
		
	}

}