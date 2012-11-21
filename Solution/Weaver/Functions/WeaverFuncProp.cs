using System;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Items;

namespace Weaver.Functions {

	//TODO: use generic type for Expression return value?

	/*================================================================================================*/
	public class WeaverFuncProp<TItem> : WeaverFunc, IWeaverProp where TItem : IWeaverItem {

		private readonly Expression<Func<TItem, object>> vProp;
		private string vPropName;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverFuncProp(Expression<Func<TItem, object>> pItemProperty) {
			vProp = pItemProperty;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string PropertyName {
			get {

				if ( vPropName != null ) { return vPropName; }
				vPropName = WeaverFuncProp.GetPropertyName(this, vProp);
				return vPropName;
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string GremlinCode {
			get { return PropertyName; }
		}

	}

	/*================================================================================================*/
	public class WeaverFuncProp {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		//stackoverflow.com/questions/12420466/
		//	unable-to-cast-object-of-type-system-linq-expressions-unaryexpression-to-type
		public static string GetPropertyName<T>(IWeaverFunc pFunc, Expression<Func<T, Object>> pExp) {

			MemberExpression me = GetMemberExpr(pExp);

			if ( me != null ) {
				return (me).Member.Name;
			}

			throw new WeaverFuncException(pFunc, "Item property expression body was of type "+
				pExp.Body.GetType().Name+", but must be of type MemberExpression.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static MemberExpression GetMemberExpr<T>(Expression<Func<T, Object>> pExp) {

			var me = (pExp.Body as MemberExpression);
			if ( me != null ) { return me; }

			var ue = (pExp.Body as UnaryExpression);
			return (ue != null ? (ue.Operand as MemberExpression) : null);
		}

	}

}