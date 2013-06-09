using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Path {
	
	/*================================================================================================*/
	public interface IWeaverPathPipe {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery Count();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery Iterate();

		/*--------------------------------------------------------------------------------------------* /
		IWeaverQuery Prop<T>(Expression<Func<T, object>> pProperty) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		IWeaverQuery ToQuery();


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T As<T>(out IWeaverStepAs<T> pAlias) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Back<T>(IWeaverStepAs<T> pAlias) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Has<T>(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, object pValue)
																			where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Has<T>(Expression<Func<T, object>> pProperty) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T HasNot<T>(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, object pValue) 
																			where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T HasNot<T>(Expression<Func<T, object>> pProperty)where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------* /
		T UpdateEach<T>(WeaverUpdates<T> pUpdates) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------* /
		T RemoveEach<T>() where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------* /
		T Table<T>(IWeaverTableVarAlias pAlias, WeaverTableColumns pColumns) where T : IWeaverPathItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T Next<T>(int pCount) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Aggregate<T>(IWeaverVarAlias pVar) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Retain<T>(IWeaverVarAlias pVar) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Except<T>(IWeaverVarAlias pVar) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Iterate<T>() where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Dedup<T>() where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T Limit<T>(int pStartIndex, int pEndIndex) where T : IWeaverElement;

		/*--------------------------------------------------------------------------------------------*/
		T AtIndex<T>(int pStartIndex) where T : IWeaverElement;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T CustomStep<T>(string pScript, bool pSkipDotPrefix=false) where T : IWeaverElement;

	}

}