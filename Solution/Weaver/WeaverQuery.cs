using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverQuery : IWeaverQuery {

		public bool IsFinalized { get; private set; }
		public string Script { get; private set; }
		public Dictionary<string, string> Params { get; private set; }
		public IWeaverVarAlias ResultVar { get; private set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverQuery() {
			Params = new Dictionary<string, string>();
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
		public void StoreResultAsVar(IWeaverVarAlias pVarAlias) { //TEST: WeaverQuery.StoreResultAsVar
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
		public void AddParam(string pParamName, IWeaverQueryVal pValue) {
			Params.Add(pParamName, pValue.GetQuoted());
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParam(IWeaverQueryVal pValue) {
			string p = NextParamName;
			AddParam(p, pValue);
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParamIfString(IWeaverQueryVal pValue) {
			if ( !pValue.IsString ) { return pValue.FixedText; }
			return AddParam(pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string NextParamName {
			get { return "_P"+Params.Keys.Count; }
		}

	}

}