using System.Collections.Generic;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;

namespace Weaver.Core.Path {
	
	/*================================================================================================*/
	public class WeaverPath : IWeaverPath {

		public IWeaverConfig Config { get; private set; }
		public IWeaverQuery Query { get; private set; }

		protected readonly IList<IWeaverPathItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverConfig pConfig, IWeaverQuery pQuery) {
			Config = pConfig;
			Query = pQuery;
			vItems = new List<IWeaverPathItem>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddItem(IWeaverPathItem pItem) {
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

			foreach ( IWeaverPathItem item in vItems ) {
				s += (s == "" || item.SkipDotPrefix ? "" : ".")+item.BuildParameterizedString();
			}

			return s;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public int Length { get { return vItems.Count; } }

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathItem ItemAtIndex(int pIndex) {
			ThrowIfOutOfBounds(pIndex);
			return vItems[pIndex];
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverPathItem> PathToIndex(int pIndex, bool pInclusive=true) {
			pIndex -= (pInclusive ? 0 : 1);
			ThrowIfOutOfBounds(pIndex);

			var path = new List<IWeaverPathItem>();

			for ( int i = 0 ; i <= pIndex ; ++i ) {
				path.Add(vItems[i]);
			}

			return path;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<IWeaverPathItem> PathFromIndex(int pIndex, bool pInclusive=true) {
			pIndex += (pInclusive ? 0 : 1);
			ThrowIfOutOfBounds(pIndex);

			var path = new List<IWeaverPathItem>();
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
		public int IndexOfItem(IWeaverPathItem pItem) {
			return vItems.IndexOf(pItem);
		}
		
	}

}