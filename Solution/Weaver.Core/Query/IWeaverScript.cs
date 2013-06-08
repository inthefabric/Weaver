using System.Collections.Generic;

namespace Weaver.Core.Query {

	/*================================================================================================*/
	public interface IWeaverScript {

		string Script { get; }
		Dictionary<string, IWeaverQueryVal> Params { get; }

	}

}