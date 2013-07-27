using System.Collections.Generic;
using System.Text.RegularExpressions;
using Weaver.Core.Exceptions;

namespace Weaver.Core.Query {

	/*================================================================================================*/
	public class WeaverTransaction : IWeaverTransaction {

		public string Script { get; set; }
		public Dictionary<string, IWeaverQueryVal> Params { get; set; }

		public IList<IWeaverQuery> Queries { get; private set; }
		private int vVarCount;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public WeaverTransaction() {
			Queries = new List<IWeaverQuery>();
			vVarCount = -1;
		}

		/*--------------------------------------------------------------------------------------------*/
		public IWeaverQuery AddQuery(IWeaverQuery pQuery) {
			EnsureUnfinished();

			if ( pQuery == null ) {
				throw new WeaverException("Cannot add a null query to transaction.");
			}

			Queries.Add(pQuery);
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

			foreach ( IWeaverQuery wq in Queries ) {
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