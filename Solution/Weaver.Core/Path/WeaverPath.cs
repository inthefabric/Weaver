using System.Collections.Generic;
using Weaver.Core.Exceptions;
using Weaver.Core.Items;
using Weaver.Core.Query;

namespace Weaver.Core.Path {
	
	/*================================================================================================*/
	public class WeaverPath : IWeaverPath {

		public IWeaverConfig Config { get; private set; }
		public IWeaverQuery Query { get; private set; }

		protected readonly IList<IWeaverItem> vItems;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverPath(IWeaverConfig pConfig, IWeaverQuery pQuery) {
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

}