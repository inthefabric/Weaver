using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Core.Steps;

namespace Weaver.Core.Path {
	
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
		public IWeaverQuery ToQuery() {
			Path.Query.FinalizeQuery(Path.BuildParameterizedScript());
			return Path.Query;
		}

	}


	/*================================================================================================*/
	public abstract class WeaverPathPipe<T> : WeaverPathPipe, IWeaverPathPipe<T>
																	where T : class, IWeaverElement {

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Self {
			get {
				T self = (this as T);

				if ( self == null ) {
					throw new WeaverPathException(Path, "Invalid type for path item "+PathIndex+": "+
					GetType().Name+". Expected type: "+typeof(T)+".");
				}

				return self;
			}
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
			var f = new WeaverStepCustom("next("+pCount+")");
			Path.AddItem(f);
			return Self;
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