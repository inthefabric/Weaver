using System.Collections.Generic;
using System.Text.RegularExpressions;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverTransaction : IWeaverTransaction {

		public string Script { get; set; }
		public Dictionary<string, IWeaverQueryVal> Params { get; set; }

		private readonly List<IWeaverQuery> vQueries;
		private int vVarCount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTransaction() {
			vQueries = new List<IWeaverQuery>();
			vVarCount = -1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddQuery(IWeaverQuery pQuery) {
			EnsureUnfinished();

			if ( pQuery == null ) {
				throw new WeaverException("Cannot add a null query to transaction.");
			}

			vQueries.Add(pQuery);
			return pQuery;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string GetNextVarName() {
			return "_V"+(++vVarCount);
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverTransaction Finish(IWeaverVarAlias pFinalOutput=null) {
			EnsureUnfinished();

			Script = "";
			Params = new Dictionary<string, IWeaverQueryVal>();
			
			const string end = @"(?=$|[^\d])";

			foreach ( IWeaverQuery wq in vQueries ) {
				string s = wq.Script + "";
				Dictionary<string, IWeaverQueryVal> pars = wq.Params;

				foreach ( string key in pars.Keys ) {
					string newKey = "_TP"+Params.Keys.Count;
					s = Regex.Replace(s, key+end, newKey);
					Params.Add(newKey, pars[key]);
				}

				Script += s;
			}

			if ( pFinalOutput != null ) {
				Script += pFinalOutput.Name+";";
			}

			return this;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void EnsureUnfinished() {
			if ( Script == null ) { return; }
			throw new WeaverException("Transaction has already been finished.");
		}

	}

}