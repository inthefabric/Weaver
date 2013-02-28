using System;
using System.Linq.Expressions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverTableColumn { //TEST: WeaverTableColumn

		public bool IsNull { get; protected set; }
		public string PropName { get; protected set; }
		public Type PropForType { get; protected set; }
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverTableColumn() {}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverTableColumn Build<T>(bool pIsNull) where T : IWeaverItemIndexable {
			var col = new WeaverTableColumn();
			col.IsNull = pIsNull;
			col.PropForType = typeof(T);
			return col;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static WeaverTableColumn Build<T>(Expression<Func<T, object>> pItemProperty)
																		where T : IWeaverItemIndexable {
			var col = new WeaverTableColumn();
			col.PropName = WeaverUtil.GetPropertyName(pItemProperty);
			col.PropForType = typeof(T);
			return col;
		}

	}

}