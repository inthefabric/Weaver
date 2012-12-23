using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverQuery {

		string Script { get; set; }
		Dictionary<string, string> Params { get; set; }

		/*--------------------------------------------------------------------------------------------*/
		void AddParam(string pParamName, WeaverQueryVal pValue);
		string AddParam(WeaverQueryVal pValue);
		string AddParamIfString(WeaverQueryVal pValue);
		string NextParamName { get; }

	}

}