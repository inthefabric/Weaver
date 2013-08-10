using System.Collections.Generic;
using System.Text;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Elements;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepTable : WeaverStep { //TEST: WeaverStepTable

		public IWeaverVarAlias Alias { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepTable(IWeaverElement pElem, out IWeaverVarAlias pAlias) {
			Alias = pAlias = new WeaverVarAlias("_TABLE"+pElem.Path.Length);
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			int thisI = Path.IndexOfItem(this);
			var sb = new StringBuilder("table("+Alias.Name+"=new Table())");
			var asSteps = new List<IWeaverStepAs>();
			int prevI = -1;
			int prevLevel = -1;

			// Collect all the "as" aliases. Reverse the order of any contiguous aliases. This matches
			// the odd Gremlin functionality (possibly a bug). This results in (pseudocode):
			// g.v(1).as('id').as('name').table(t){it.name}{it.id}; //note the id/name order swap

			for ( int i = 0 ; i < thisI ; ++i ) {
				IWeaverPathItem item = Path.ItemAtIndex(i);
				IWeaverStepAs a = (item as IWeaverStepAs);

				if ( a == null ) {
					continue;
				}

				if ( i-1 == prevI ) {
					asSteps.Insert(prevLevel, a);
				}
				else {
					asSteps.Add(a);
					prevLevel = asSteps.Count-1;
				}

				prevI = i;
			}

			// Append column closures for each "as" alias. Non-column aliases are given the empty "{}"
			// closure. The column can be property or object based, and the closure script can 
			// (optionally) can be appended or replaced.

			foreach ( IWeaverStepAs a in asSteps ) {
				IWeaverStepAsColumn col = (a as IWeaverStepAsColumn);
				if ( col == null ) {
					sb.Append("{}");
					continue;
				}
				
				if ( col.ReplaceScript != null ) {
					sb.Append("{"+col.ReplaceScript+"}");
					continue;
				}
				
				if ( col.PropName != null ) {
					string p = Path.Query.AddParam(new WeaverQueryVal(col.PropName));
					sb.Append("{it.getProperty("+p+")"+col.AppendScript+"}");
					continue;
				}

				sb.Append("{it"+col.AppendScript+"}");
			}

			return sb.ToString();
		}

	}

}