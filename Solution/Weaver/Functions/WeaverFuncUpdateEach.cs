using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncUpdateEach<TItem> : WeaverFunc where TItem : IWeaverItemIndexable {

		private readonly WeaverUpdates<TItem> vUpdates;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncUpdateEach(WeaverUpdates<TItem> pUpdates) {
			vUpdates = pUpdates;

			if ( vUpdates.Count == 0 ) {
				throw new WeaverFuncException(this, "WeaverUpdates cannot be empty.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			IWeaverQuery q = Path.Query;
			string each = "each{";

			for ( int i = 0 ; i < vUpdates.Count ; ++i ) {
				KeyValuePair<string, WeaverQueryVal> pair = vUpdates[i];
				each += (i == 0 ? "" : ";")+"it."+pair.Key+"="+q.AddParamIfString(pair.Value);
			}

			return each+"}";
		}

	}

}