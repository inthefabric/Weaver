using System.Collections.Generic;
using Weaver.Core.Elements;
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


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery StoreResultAsVar(string pName, IWeaverQuery pQuery,
																			out IWeaverVarAlias pVar) {
			pVar = new WeaverVarAlias(pName);
			return StoreResultInner(pQuery, pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery StoreResultAsVar<T>(string pName, IWeaverQuery pQuery,
												out IWeaverVarAlias<T> pVar) where T : IWeaverElement {
			pVar = new WeaverVarAlias<T>(pName);
			return StoreResultInner(pQuery, pVar);
		}

		/*--------------------------------------------------------------------------------------------*/
		private static IWeaverQuery StoreResultInner(IWeaverQuery pQuery, IWeaverVarAlias pVarAlias) {
			if ( !pQuery.IsFinalized ) {
				throw new WeaverException("Query must be finalized.");
			}

			if ( pQuery.ResultVar != null ) {
				throw new WeaverException(
					"Query result already stored as '"+pQuery.ResultVar.Name+"'.");
			}

			var s = pQuery.Script;

			var q = new WeaverQuery();
			q.ResultVar = pVarAlias;
			q.Params = pQuery.Params;
			q.FinalizeQuery(pVarAlias.Name+"="+s.Substring(0, s.Length-1));
			return q;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery InitListVar(string pName, IList<IWeaverVarAlias> pVars,
																			out IWeaverVarAlias pVar) {
			pVar = new WeaverVarAlias(pName);
			string list = "";

			foreach ( IWeaverVarAlias var in pVars ) {
				list += (list == "" ? "" : ",")+var.Name;
			}

			var q = new WeaverQuery();
			q.FinalizeQuery(pVar.Name+"=["+list+"]");
			return q;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static IWeaverQuery InitListVar(string pName, out IWeaverVarAlias pVar) {
			return InitListVar(pName, new List<IWeaverVarAlias>(), out pVar);
		}

	}

}