using System.Collections.Generic;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class ResponseCmd {

		public long Timer { get; set; }
		public IList<JsonObject> Results { get; set; }
		public string Err { get; set; }

	}

}