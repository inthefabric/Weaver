using System;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	//TODO: use generic type for Expression return value?

	/*================================================================================================*/
	public class WeaverFuncProp<TItem> : WeaverFunc<TItem>, IWeaverProp where TItem : IWeaverItem {

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
				vPropName = GetPropertyName(this, vProp);
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
		public static string GetPropertyName<T>(IWeaverItem pCaller, Expression<Func<T, Object>> pExp) {
			var me = (pExp.Body as MemberExpression);

			if ( me != null ) {
				return (me).Member.Name;
			}

			var ue = (pExp.Body as UnaryExpression);

			if ( ue != null ) {
				return ((MemberExpression)ue.Operand).Member.Name;
			}

			throw new WeaverGremlinException(pCaller, "Item property expression was of type "+
				pExp.Body.GetType().Name+", but must be of type MemberExpression or UnaryExpression.");
		}

	}

}