using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps.Helpers;

namespace Weaver.Core.Steps {

	/*================================================================================================*/
	public class WeaverStepTable : WeaverStep { //TEST: WeaverStepTable

		public IWeaverVarAlias Alias { get; private set; }
		public WeaverTableColumns Columns { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverStepTable(IWeaverVarAlias pAlias, WeaverTableColumns pColumns) {
			Alias = pAlias;
			Columns = pColumns;

			if ( Columns.Count == 0 ) {
				throw new WeaverStepException(this, "WeaverTableColumns cannot be empty.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			int thisI = Path.IndexOfItem(this);
			int colI = 0;
			string script = "table("+Alias.Name+")";

			for ( int i = 0 ; i < thisI ; ++i ) {
				IWeaverPathItem item = Path.ItemAtIndex(i);
				IWeaverStepAs fa = (item as IWeaverStepAs);

				if ( fa == null ) {
					continue;
				}

				WeaverTableColumn col = Columns[colI];

				if ( !fa.ItemType.IsAssignableFrom(col.PropForType) ) {
					throw new WeaverStepException(this, "Table column at index "+colI+" expects type '"+
						col.PropForType.Name+"', but matches alias '"+fa.Label+"' of type '"+
						fa.ItemType.Name+"'.");
				}

				col.Alias = fa.Label;

				if ( col.IsNull ) {
					script += "{}";
				}
				else if ( col.PropName != null ) {
					string propParam = Path.Query.AddParam(new WeaverQueryVal(col.PropName));
					script += "{it.property("+propParam+")"+(col.PropScript ?? "")+"}";
				}
				else {
					script += "{it}";
				}

				colI++;
			}

			if ( colI != Columns.Count ) {
				throw new WeaverStepException(this, colI+" of "+Columns.Count+" column(s) were added "+
					"to the table. Ensure that an 'As' alias exists for each table column.");
			}

			return script;
		}

	}

}