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
		void AddParam(string pParamName, IWeaverQueryVal pValue);
		string AddParam(IWeaverQueryVal pValue);
		string AddParamIfString(IWeaverQueryVal pValue);
		string NextParamName { get; }

	}

}