using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Path {

	/*================================================================================================*/
	public abstract class WeaverPathPipe : IWeaverPathPipe {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery Count() {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery Iterate() {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery ToQuery() {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T As<T>(out IWeaverStepAs<T> pAlias) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Back<T>(IWeaverStepAs<T> pAlias) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has<T>(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, 
															object pValue) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has<T>(Expression<Func<T, object>> pProperty) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot<T>(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation,
															object pValue) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot<T>(Expression<Func<T, object>> pProperty) where T : IWeaverElement {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Next<T>(int pCount) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Aggregate<T>(IWeaverVarAlias pVar) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Retain<T>(IWeaverVarAlias pVar) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Except<T>(IWeaverVarAlias pVar) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Iterate<T>() where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Dedup<T>() where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Limit<T>(int pStartIndex, int pEndIndex) where T : IWeaverElement {
			throw new NotImplementedException();
		}

		/*--------------------------------------------------------------------------------------------*/
		public T AtIndex<T>(int pStartIndex) where T : IWeaverElement {
			throw new NotImplementedException();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T CustomStep<T>(string pScript, bool pSkipDotPrefix = false) where T : IWeaverElement {
			throw new NotImplementedException();
		}

	}

}