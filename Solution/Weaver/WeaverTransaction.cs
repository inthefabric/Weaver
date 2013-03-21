using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverTransaction : IWeaverTransaction {

		public enum ConclusionType {
			Success = 1,
			Failure
		}

		public string Script { get; set; }
		public Dictionary<string, string> Params { get; set; }
		public ConclusionType Conclusion { get; set; }

		private readonly List<IWeaverQuery> vQueries;
		private int vVarCount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTransaction() {
			vQueries = new List<IWeaverQuery>();
			vVarCount = -1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public void AddQuery(IWeaverQuery pQuery) {
			EnsureUnfinished();

			if ( pQuery == null ) {
				throw new WeaverException("Cannot add a null query to transaction.");
			}

			vQueries.Add(pQuery);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetNextVarName() {
			return "_V"+(++vVarCount);
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Finish(ConclusionType pConclusion, IWeaverVarAlias pFinalOutput=null) {
			FinishWithoutStartStop();

			Script = "g.startTransaction();"+
				Script+
				"g.stopTransaction(TransactionalGraph.Conclusion." +
				pConclusion.ToString().ToUpper() + ");";

			if ( pFinalOutput != null ) {
				Script += pFinalOutput.Name+";";
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		//TEST: WeaverTransaction.FinishWithConclusion()
		public void FinishWithConclusion(ConclusionType pConclusion, IWeaverVarAlias pFinalOutput=null){
			FinishWithoutStartStop();

			Script = Script+"g.stopTransaction(TransactionalGraph.Conclusion." +
				pConclusion.ToString().ToUpper() + ");";

			if ( pFinalOutput != null ) {
				Script += pFinalOutput.Name+";";
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public void FinishWithoutStartStop(IWeaverVarAlias pFinalOutput=null) {
			EnsureUnfinished();

			/*if ( vQueries.Count == 0 ) {
				throw new WeaverException("Could not finish transaction; no queries were added.");
			}*/

			Script = "";
			Params = new Dictionary<string, string>();

			foreach ( IWeaverQuery wq in vQueries ) {
				string queryScript = wq.Script + "";
				Dictionary<string, string> pars = wq.Params;

				foreach ( string key in pars.Keys ) {
					string newKey = "_TP" + Params.Keys.Count;
					queryScript = queryScript.Replace(key, newKey);
					Params.Add(newKey, pars[key]);
				}

				Script += queryScript;
			}

			if ( pFinalOutput != null ) {
				Script += pFinalOutput.Name+";";
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void EnsureUnfinished() {
			if ( Script == null ) { return; }
			throw new WeaverException("Transaction has already been finished.");
		}

	}

}