using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;

namespace Weaver.Items {

	/*================================================================================================*/
	public abstract class WeaverItem : Object, IWeaverItem {
		
		private int vQueryPathIndex;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected WeaverItem() {
			vQueryPathIndex = -1;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPath Path { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		public int PathIndex {
			get {
				if ( Path == null ) {
					throw new WeaverException("Path is null for "+this+".");
				}

				if ( vQueryPathIndex > -1 ) { return vQueryPathIndex; }
				vQueryPathIndex = Path.IndexOfItem(this);
				return vQueryPathIndex;
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem PrevQueryItem {
			get { return Path.ItemAtIndex(PathIndex-1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem NextQueryItem {
			get { return Path.ItemAtIndex(PathIndex+1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> QueryPathToThisItem {
			get { return Path.PathToIndex(PathIndex); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> QueryPathFromThisItem {
			get { return Path.PathFromIndex(PathIndex); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string ItemIdentifier { get { return this+""; } }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract string GremlinCode { get; }

	}


	/*================================================================================================*/
	public static class WeaverItemExtensions {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T As<T>(this T pCallingItem, out T pAlias) where T : IWeaverItem {
			var func = new WeaverFuncAs<T>(pCallingItem.Path);
			pCallingItem.Path.AddItem(func);
			pAlias = pCallingItem;
			return pCallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBack Back<T, TBack>(this T pCallingItem, TBack pAlias) where T : IWeaverItem
																			where TBack : IWeaverItem {
			var func = new WeaverFuncBack<TBack>(pCallingItem.Path, pAlias);
			pCallingItem.Path.AddItem(func);
			return pAlias;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverProp Prop<T>(this T pCallingItem,
									Expression<Func<T, object>> pItemProperty) where T : IWeaverItem {
			var func = new WeaverFuncProp<T>(pItemProperty);
			pCallingItem.Path.AddItem(func);
			return func;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pCallingItem, Expression<Func<T, object>> pItemProperty,
						WeaverFuncHasOp pOperation, object pValue) where T : IWeaverItem {
			var func = new WeaverFuncHas<T>(pItemProperty, pOperation, pValue);
			pCallingItem.Path.AddItem(func);
			return pCallingItem;
		}

	}

}