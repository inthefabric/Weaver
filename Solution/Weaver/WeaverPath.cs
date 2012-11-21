using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver {

	//TODO: indexing, see stackoverflow.com/a/10073156

	/*================================================================================================*/
	public class WeaverPath : IWeaverPath {

		public IWeaverNode BaseNode { get; private set; }
		private readonly IList<IWeaverItem> vItems;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverNode pBaseNode) {
			BaseNode = pBaseNode;
			vItems = new List<IWeaverItem>();
			AddItem(BaseNode);
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
		public static string GetGremlinCode(WeaverPath pPath) {
			return GetGremlinCode(pPath.vItems);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetGremlinCode(IList<IWeaverItem> pPathItems) {
			string gremlin = "g.";

			foreach ( IWeaverItem q in pPathItems ) {
				gremlin += q.GremlinCode+'.';
			}

			return gremlin.Substring(0, gremlin.Length-1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GremlinCode { get { return GetGremlinCode(vItems); } }

	}

}