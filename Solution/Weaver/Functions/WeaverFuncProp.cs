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
		public static string GetPropertyName<T>(IWeaverFunc pFunc, Expression<Func<T, object>> pExp) {

			try {
				return GetPropertyName(pExp);
			}
			catch ( WeaverException we ) {
				throw new WeaverFuncException(pFunc, we.Message);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public static string GetPropertyName<T>(Expression<Func<T, object>> pExp) {

			MemberExpression me = GetMemberExpr(pExp);

			if ( me != null ) {
				return (me).Member.Name;
			}

			throw new WeaverException("Item property expression body was of type "+
				pExp.Body.GetType().Name+", but must be of type MemberExpression.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static MemberExpression GetMemberExpr<T>(Expression<Func<T, object>> pExp) {

			var me = (pExp.Body as MemberExpression);
			if ( me != null ) { return me; }

			var ue = (pExp.Body as UnaryExpression);
			return (ue != null ? (ue.Operand as MemberExpression) : null);
		}

	}

}