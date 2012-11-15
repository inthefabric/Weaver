using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fabric.Domain.Graph.Functions;
using Fabric.Domain.Graph.Interfaces;

namespace Fabric.Domain.Graph.Items {

	/*================================================================================================*/
	public abstract class WeaverItem : Object, IWeaverItem {

		public int QueryPathIndex { get; private set; }
		
		private WeaverQuery vQuery;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery Query {
			get {
				return vQuery;
			}
			set {
				vQuery = value;
				QueryPathIndex = vQuery.PathLength();
				vQuery.AddQueryItem(this);
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem PrevQueryItem {
			get { return vQuery.PathAtIndex(QueryPathIndex-1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IWeaverItem NextQueryItem {
			get { return vQuery.PathAtIndex(QueryPathIndex+1); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> QueryPathToThisItem {
			get { return vQuery.PathToIndex(QueryPathIndex); }
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IWeaverItem> QueryPathFromThisItem {
			get { return vQuery.PathFromIndex(QueryPathIndex); }
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TItem As<TItem>(string pLabel) where TItem : IWeaverItem {
			var func = new WeaverFuncAs<TItem>(this, pLabel) { Query = Query };
			return func.CallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public TToItem Back<TToItem>(string pLabel) where TToItem : IWeaverItem {
			TToItem toNode = (TToItem)Query.FindAsNode<TToItem>(pLabel);
			var func = new WeaverFuncBack<TToItem>(toNode, pLabel) { Query = Query };
			return func.CallingItem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverProp Prop<TItem>(Expression<Func<TItem, object>> pItemProperty)
																		where TItem : IWeaverItem {
			var func = new WeaverFuncProp<TItem>(this, pItemProperty) { Query = Query };
			return func;
		}

		/*--------------------------------------------------------------------------------------------*/
		public TItem Has<TItem>(Expression<Func<TItem, object>> pItemProperty,
						WeaverFuncHasOp pOperation, object pValue) where TItem : IWeaverItem {
			var func = new WeaverFuncHas<TItem>(this, pItemProperty, pOperation, pValue) 
				{ Query = Query };
			return func.CallingItem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public abstract string GremlinCode { get; }

	}


	/*================================================================================================* /
	public static class WeaverItemExtensions {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public static TItem Has2<TItem>(this TItem pCallingItem,
						Expression<Func<TItem, object>> pItemProperty, WeaverFuncHasOp pOperation,
						object pValue) where TItem : IWeaverItem {

			var func = new WeaverFuncHas<TItem>(
				pCallingItem, pItemProperty, pOperation, pValue) { Query = pCallingItem.Query };

			return func.CallingItem;
		}

	}*/

}