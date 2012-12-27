using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {

		bool IsFinalized { get; }
		string Script { get; }
		Dictionary<string, string> Params { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void FinalizeQuery(string pScript);

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, WeaverQueryVal pValue);
		string AddParam(WeaverQueryVal pValue);
		string AddParamIfString(WeaverQueryVal pValue);
		string NextParamName { get; }

	}

}