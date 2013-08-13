using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;

namespace Weaver.Core.Pipe {
	
	/*================================================================================================*/
	public abstract class WeaverPathPipe : WeaverPathItem, IWeaverPathPipe {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd Count() {
			var f = new WeaverStepCustom("count()");
			Path.AddItem(f);
			return f;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd Iterate() {
			var f = new WeaverStepCustom("iterate()");
			Path.AddItem(f);
			return f;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd Remove() {
			var f = new WeaverStepCustom("remove()");
			Path.AddItem(f);
			return f;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery ToQuery() {
			Path.Query.FinalizeQuery(Path.BuildParameterizedScript());
			return Path.Query;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery ToQueryAsVar(string pName, out IWeaverVarAlias pVar) {
			Path.Query.FinalizeQuery(Path.BuildParameterizedScript());
			return WeaverQuery.StoreResultAsVar(pName, Path.Query, out pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery ToQueryAsVar<T>(string pName, out IWeaverVarAlias<T> pVar)
																			where T : IWeaverElement {
			Path.Query.FinalizeQuery(Path.BuildParameterizedScript());
			return WeaverQuery.StoreResultAsVar(pName, Path.Query, out pVar);
		}

	}


	/*================================================================================================*/
	public static class WeaverPathPipeExt {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T As<T>(this T pElem, out IWeaverStepAs<T> pAlias) where T : IWeaverElement {
			pAlias = new WeaverStepAs<T>(pElem);
			pElem.Path.AddItem(pAlias);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static TBack Back<T, TBack>(this T pElem, IWeaverStepAs<TBack> pAlias)
												where T : IWeaverElement where TBack : IWeaverElement {
			var f = new WeaverStepBack<TBack>(pAlias);
			pElem.Path.AddItem(f);
			return pAlias.Item;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pElem, Expression<Func<T, object>> pProperty, 
								WeaverStepHasOp pOperation, object pValue) where T : IWeaverElement {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.Has, pOperation, pValue);
			pElem.Path.AddItem(f);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Has<T>(this T pElem, Expression<Func<T, object>> pProperty) 
																			where T : IWeaverElement {
			var r = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.Has);
			pElem.Path.AddItem(r);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T HasNot<T>(this T pElem, Expression<Func<T, object>> pProperty, 
								WeaverStepHasOp pOperation, object pValue) where T : IWeaverElement {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.HasNot, pOperation, pValue);
			pElem.Path.AddItem(f);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T HasNot<T>(this T pElem, Expression<Func<T, object>> pProperty) 
																			where T : IWeaverElement {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.HasNot);
			pElem.Path.AddItem(f);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T SideEffect<T>(this T pElem, params IWeaverStatement<T>[] pStatements)
																			where T : IWeaverElement {
			var f = new WeaverStepSideEffect<T>(pStatements);
			pElem.Path.AddItem(f);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverPathPipeEnd Property<T>(this T pElem, 
									Expression<Func<T, object>> pProperty) where T : IWeaverElement {
			var f = new WeaverStepProp<T>(pProperty);
			pElem.Path.AddItem(f);
			return f;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T AsColumn<T>(this T pElem, string pLabel, Expression<Func<T, object>> pProperty,
										out IWeaverStepAsColumn<T> pAlias) where T : IWeaverElement {
			pAlias = new WeaverStepAsColumn<T>(pElem, pElem.Path.Config, pLabel, pProperty);
			pElem.Path.AddItem(pAlias);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T AsColumn<T>(this T pElem, string pLabel, Expression<Func<T, object>> pProperty)
																			where T : IWeaverElement {
			var ac = new WeaverStepAsColumn<T>(pElem, pElem.Path.Config, pLabel, pProperty);
			pElem.Path.AddItem(ac);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T AsColumn<T>(this T pElem, string pLabel, out IWeaverStepAsColumn<T> pAlias)
																			where T : IWeaverElement {
			pAlias = new WeaverStepAsColumn<T>(pElem, pElem.Path.Config, pLabel);
			pElem.Path.AddItem(pAlias);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T AsColumn<T>(this T pElem, string pLabel) where T : IWeaverElement {
			var ac = new WeaverStepAsColumn<T>(pElem, pElem.Path.Config, pLabel);
			pElem.Path.AddItem(ac);
			return pElem;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Table<T>(this T pElem, out IWeaverVarAlias pAlias) where T : IWeaverElement {
			var t = new WeaverStepTable(pElem, out pAlias);
			pElem.Path.AddItem(t);
			return pElem;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T Next<T>(this T pElem, int? pCount=null) where T : IWeaverElement {
			return CustomStep(pElem, "next("+(pCount == null ? "" : pCount+"")+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Aggregate<T>(this T pElem, IWeaverVarAlias pVar) where T : IWeaverElement {
			return CustomStep(pElem, "aggregate("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Retain<T>(this T pElem, IWeaverVarAlias pVar) where T : IWeaverElement {
			return CustomStep(pElem, "retain("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Except<T>(this T pElem, IWeaverVarAlias pVar) where T : IWeaverElement {
			return CustomStep(pElem, "except("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Dedup<T>(this T pElem) where T : IWeaverElement {
			return CustomStep(pElem, "dedup");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T Limit<T>(this T pElem, int pStartIndex, int pEndIndex) where T : IWeaverElement{
			return CustomStep(pElem, "["+pStartIndex+".."+pEndIndex+"]", true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static T AtIndex<T>(this T pElem, int pIndex) where T : IWeaverElement {
			return CustomStep(pElem, "["+pIndex+"]", true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static T CustomStep<T>(this T pElem, string pScript, bool pSkipDotPrefix=false)
																			where T : IWeaverElement {
			var f = new WeaverStepCustom(pScript, pSkipDotPrefix);
			pElem.Path.AddItem(f);
			return pElem;
		}

	}

}