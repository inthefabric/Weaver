using System;
using System.Linq.Expressions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	/*================================================================================================*/
	public class WeaverFuncProp<TItem> : WeaverFunc<TItem>, IWeaverProp 
																		where TItem : IWeaverItem {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncProp(IWeaverItem pCallingItem, 
								Expression<Func<TItem, object>> pItemProperty) : base(pCallingItem) {
			vProp = pItemProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {

				if ( vPropName != null ) { return vPropName; }
				vPropName = GetPropertyName(vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get { return PropertyName; }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//stackoverflow.com/questions/12420466/
		//	unable-to-cast-object-of-type-system-linq-expressions-unaryexpression-to-type
		public static string GetPropertyName<T>(Expression<Func<T, Object>> pExp) {
			/*if ( pExp.Body is MemberExpression ) {
				return ((MemberExpression)pExp.Body).Member.Name;
			}*/

			UnaryExpression body = (pExp.Body as UnaryExpression);

			if ( body == null ) {
				throw new Exception("Prop() function expected a UnaryExpression.");
			}

			var op = ((UnaryExpression)pExp.Body).Operand;
			return ((MemberExpression)op).Member.Name;
		}

	}

}