using System;
using System.Linq.Expressions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverTableColumn { //TEST: WeaverTableColumn

		public bool IsNull { get; protected set; }
		public string PropName { get; protected set; }
		public string PropScript { get; protected set; }
		public Type PropForType { get; protected set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverTableColumn() {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		internal static WeaverTableColumn Build<T>(bool pIsNull) where T : IWeaverItemIndexable {
			var col = new WeaverTableColumn();
			col.IsNull = pIsNull;
			col.PropForType = typeof(T);
			return col;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		internal static WeaverTableColumn Build<T>(IWeaverConfig pConfig, 
								Expression<Func<T, object>> pItemProperty, string pPropertyScript=null)
								where T : IWeaverItemIndexable {
			var col = new WeaverTableColumn();
			col.PropName = pConfig.GetPropertyDbName(pItemProperty);
			col.PropScript = pPropertyScript;
			col.PropForType = typeof(T);
			return col;
		}

	}

}