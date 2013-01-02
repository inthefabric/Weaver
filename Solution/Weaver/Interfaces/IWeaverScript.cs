using System.Collections.Generic;

namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverScript {

		string Script { get; }
		Dictionary<string, string> Params { get; }

	}

}