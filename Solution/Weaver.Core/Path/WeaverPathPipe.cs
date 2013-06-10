using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Path {

	/*================================================================================================*/
	public abstract class WeaverPathPipe<T> : WeaverPathItem, IWeaverPathPipe<T>
																			where T : IWeaverElement {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd<T> Count() {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverPathPipeEnd<T> Iterate() {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery ToQuery() {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T As(out IWeaverStepAs<T> pAlias) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Back(IWeaverStepAs<T> pAlias) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, 
															object pValue) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has(Expression<Func<T, object>> pProperty) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation,
															object pValue) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot(Expression<Func<T, object>> pProperty) {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Next(int pCount) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Aggregate(IWeaverVarAlias pVar) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Retain(IWeaverVarAlias pVar) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Except(IWeaverVarAlias pVar) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Dedup() {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Limit(int pStartIndex, int pEndIndex) {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T AtIndex(int pStartIndex) {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T CustomStep(string pScript, bool pSkipDotPrefix = false) {
			throw new NotImplementedException();
		}

	}

}