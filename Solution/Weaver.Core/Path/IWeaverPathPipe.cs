﻿using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Path {
	
	/*================================================================================================*/
	public interface IWeaverPathPipe<T> : IWeaverPathPipeEnd<T> where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd<T> Count();

		/*--------------------------------------------------------------------------------------------*/
		IWeaverPathPipeEnd<T> Iterate();

		/*--------------------------------------------------------------------------------------------* /
		IWeaverQuery Prop(Expression<Func<T, object>> pProperty);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T As(out IWeaverStepAs<T> pAlias);

		/*--------------------------------------------------------------------------------------------*/
		T Back(IWeaverStepAs<T> pAlias);

		/*--------------------------------------------------------------------------------------------*/
		T Has(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, object pValue);

		/*--------------------------------------------------------------------------------------------*/
		T Has(Expression<Func<T, object>> pProperty);

		/*--------------------------------------------------------------------------------------------*/
		T HasNot(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, object pValue);

		/*--------------------------------------------------------------------------------------------*/
		T HasNot(Expression<Func<T, object>> pProperty);

		/*--------------------------------------------------------------------------------------------* /
		T UpdateEach(WeaverUpdates<T> pUpdates);

		/*--------------------------------------------------------------------------------------------* /
		T RemoveEach();

		/*--------------------------------------------------------------------------------------------* /
		T Table(IWeaverTableVarAlias pAlias, WeaverTableColumns pColumns) where T : IWeaverPathItem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T Next(int pCount);

		/*--------------------------------------------------------------------------------------------*/
		T Aggregate(IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		T Retain(IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		T Except(IWeaverVarAlias pVar);

		/*--------------------------------------------------------------------------------------------*/
		T Dedup();

		/*--------------------------------------------------------------------------------------------*/
		T Limit(int pStartIndex, int pEndIndex);

		/*--------------------------------------------------------------------------------------------*/
		T AtIndex(int pStartIndex);


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		T CustomStep(string pScript, bool pSkipDotPrefix=false);

	}

}