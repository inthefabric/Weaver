using System.Collections.Generic;
using Weaver.Exceptions;
using Weaver.Interfaces;

namespace Weaver {

	/*================================================================================================*/
	public class WeaverQuery : IWeaverQuery { //TEST: WeaverQuery

		public bool IsFinalized { get; private set; }
		public string Script { get; private set; }
		public Dictionary<string, string> Params { get; private set; }

		
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
			Script = pScript;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void AddParam(string pParamName, WeaverQueryVal pValue) {
			Params.Add(pParamName, pValue.GetQuoted());
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParam(WeaverQueryVal pValue) {
			string p = NextParamName;
			AddParam(p, pValue);
			return p;
		}

		/*--------------------------------------------------------------------------------------------*/
		public string AddParamIfString(WeaverQueryVal pValue) {
			if ( !pValue.IsString ) { return pValue.FixedText; }
			return AddParam(pValue);
		}

		/*--------------------------------------------------------------------------------------------*/
		public string NextParamName {
			get { return "_P"+Params.Keys.Count; }
		}

	}

}