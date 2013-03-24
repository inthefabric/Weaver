using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncTable : WeaverFunc { //TEST: WeaverFuncTable

		public IWeaverTableVarAlias Alias { get; private set; }
		public WeaverTableColumns Columns { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncTable(IWeaverTableVarAlias pAlias, WeaverTableColumns pColumns) {
			Alias = pAlias;
			Columns = pColumns;

			if ( Columns.Count == 0 ) {
				throw new WeaverFuncException(this, "WeaverTableColumns cannot be empty.");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			int thisI = Path.IndexOfItem(this);
			int colI = 0;
			string script = "table("+Alias.Name+")";

			for ( int i = 0 ; i < thisI ; ++i ) {
				IWeaverItem item = Path.ItemAtIndex(i);
				IWeaverFuncAs fa = (item as IWeaverFuncAs);

				if ( fa == null ) {
					continue;
				}

				WeaverTableColumn col = Columns[colI];

				if ( !fa.ItemType.IsAssignableFrom(col.PropForType) ) {
					throw new WeaverFuncException(this, "Table column at index "+colI+" expects type '"+
						col.PropForType.Name+"', but matches alias '"+fa.Label+"' of type '"+
						fa.ItemType.Name+"'.");
				}

				if ( col.IsNull ) {
					script += "{}";
				}
				else if ( col.PropName != null ) {
					script += "{it."+col.PropName+(col.PropScript ?? "")+"}";
				}
				else {
					script += "{it}";
				}

				colI++;
			}

			if ( colI != Columns.Count ) {
				throw new WeaverFuncException(this, colI+" of "+Columns.Count+" column(s) were added "+
					"to the table. Ensure that an 'As' alias exists for each table column.");
			}

			return script;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IList<string> GetColumnNames() {
			int thisI = Path.IndexOfItem(this);
			var list = new List<string>();

			for ( int i = 0 ; i < thisI ; ++i ) {
				IWeaverItem item = Path.ItemAtIndex(i);
				IWeaverFuncAs fa = (item as IWeaverFuncAs);

				if ( fa != null ) {
					list.Add(fa.Label);
				}
			}

			return list;
		}

	}

}