using System;
using System.Linq.Expressions;
using Weaver.Core.Elements;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;

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
		protected T Self {
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
			pAlias = new WeaverStepAs<T>(Self);
			Path.AddItem(pAlias);
			return Self;
		}

		/*--------------------------------------------------------------------------------------------*/
		public TBack Back<TBack>(IWeaverStepAs<TBack> pAlias) where TBack : IWeaverElement {
			var f = new WeaverStepBack<TBack>(pAlias);
			Path.AddItem(f);
			return pAlias.Item;
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation, object pValue) {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.Has, pOperation, pValue);
			Path.AddItem(f);
			return Self;
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Has(Expression<Func<T, object>> pProperty) {
			var r = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.Has);
			Path.AddItem(r);
			return Self;
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot(Expression<Func<T, object>> pProperty, WeaverStepHasOp pOperation,
																						object pValue) {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.HasNot, pOperation, pValue);
			Path.AddItem(f);
			return Self;
		}

		/*--------------------------------------------------------------------------------------------*/
		public T HasNot(Expression<Func<T, object>> pProperty) {
			var f = new WeaverStepHas<T>(pProperty, WeaverStepHasMode.HasNot);
			Path.AddItem(f);
			return Self;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T Next(int? pCount=null) {
			return CustomStep("next("+(pCount == null ? "" : pCount+"")+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Aggregate(IWeaverVarAlias pVar) {
			return CustomStep("aggregate("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Retain(IWeaverVarAlias pVar) {
			return CustomStep("retain("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Except(IWeaverVarAlias pVar) {
			return CustomStep("except("+pVar.Name+")");
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Dedup() {
			return CustomStep("dedup");
		}

		/*--------------------------------------------------------------------------------------------*/
		public T Limit(int pStartIndex, int pEndIndex) {
			return CustomStep("["+pStartIndex+".."+pEndIndex+"]", true);
		}

		/*--------------------------------------------------------------------------------------------*/
		public T AtIndex(int pIndex) {
			return CustomStep("["+pIndex+"]", true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public T CustomStep(string pScript, bool pSkipDotPrefix=false) {
			var f = new WeaverStepCustom(pScript, pSkipDotPrefix);
			Path.AddItem(f);
			return Self;
		}

	}

}