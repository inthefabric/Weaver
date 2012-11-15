using System.Collections.Generic;
using Fabric.Domain.Graph.Functions;
using Fabric.Domain.Graph.Interfaces;

namespace Fabric.Domain.Graph {

	//TODO: indexing, see stackoverflow.com/a/10073156

	/*================================================================================================*/
	public class WeaverQuery {

		public IWeaverNode BaseNode { get; private set; }
		private readonly IList<IWeaverItem> vQueryPath;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery(IWeaverNode pBaseNode) {
			BaseNode = pBaseNode;
			vQueryPath = new List<IWeaverItem>();
			BaseNode.Query = this;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddQueryItem(IWeaverItem pItem) {
			vQueryPath.Add(pItem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int PathLength() {
			return vQueryPath.Count;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverItem PathAtIndex(int pIndex) {
			if ( pIndex < 0 || pIndex >= vQueryPath.Count ) { return null; }
			return vQueryPath[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathToIndex(int pIndex, bool pInclusive=true) {
			if ( pIndex < 0 || pIndex >= vQueryPath.Count ) { return null; }
			var path = new List<IWeaverItem>();
			pIndex -= (pInclusive ? 0 : 1);

			for ( int i = 0 ; i <= pIndex ; ++i ) {
				if ( i == pIndex && !pInclusive ) { break; }
				path.Add(vQueryPath[i]);
			}

			return path;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverItem> PathFromIndex(int pIndex, bool pInclusive=true) {
			if ( pIndex < 0 || pIndex >= vQueryPath.Count ) { return null; }
			var path = new List<IWeaverItem>();
			var n = vQueryPath.Count;
			pIndex += (pInclusive ? 0 : 1);

			for ( int i = pIndex ; i < n ; ++i ) {
				path.Add(vQueryPath[i]);
			}

			return path;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverItem FindAsNode<TItem>(string pLabel) where TItem : IWeaverItem {
			var n = vQueryPath.Count;

			for ( int i = 1 ; i < n ; ++i ) {
				IWeaverItem item = vQueryPath[i];
				WeaverFuncAs<TItem> funcAs = (item as WeaverFuncAs<TItem>);
				if ( funcAs == null || funcAs.Label != pLabel ) { continue; }
				return vQueryPath[i-1];
			}

			return null;
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static string GetGremlinCode(IList<IWeaverItem> pQueryPath) {
			string gremlin = "g.";

			foreach ( IWeaverItem q in pQueryPath ) {
				gremlin += q.GremlinCode+'.';
			}

			return gremlin.Substring(0, gremlin.Length-1);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GremlinCode { get { return GetGremlinCode(vQueryPath); } }

	}

}