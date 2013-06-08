﻿using System.Collections.Generic;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Query {

	/*================================================================================================*/
	public class WeaverQuery : IWeaverQuery {

		public bool IsFinalized { get; private set; }
		public string Script { get; private set; }
		public Dictionary<string, IWeaverQueryVal> Params { get; private set; }
		public IWeaverVarAlias ResultVar { get; private set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery() {
			Params = new Dictionary<string, IWeaverQueryVal>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FinalizeQuery(string pScript) {
			if ( IsFinalized ) {
				throw new WeaverException("Query is already finalized.");
			}

			IsFinalized = true;
			Script = pScript+";";
		}

		/*--------------------------------------------------------------------------------------------*/
		public void StoreResultAsVar(IWeaverVarAlias pVarAlias) {
			if ( !IsFinalized ) {
				throw new WeaverException("Query must be finalized.");
			}

			if ( ResultVar != null ) {
				throw new WeaverException("Query result already stored as '"+ResultVar.Name+"'.");
			}

			ResultVar = pVarAlias;
			Script = ResultVar.Name+"="+Script;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public string AddStringParam(string pValue) {
			return AddParamInner(new WeaverQueryVal(pValue));
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParam(IWeaverQueryVal pValue) {
			return AddParamInner(pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		private string AddParamInner(IWeaverQueryVal pValue) {
			string p = "_P"+Params.Keys.Count;
			Params.Add(p, pValue);
			return p;
		}

	}

}