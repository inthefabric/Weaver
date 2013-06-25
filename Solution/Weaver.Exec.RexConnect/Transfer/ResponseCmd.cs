using System.Collections.Generic;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class ResponseCmd {

		public virtual long Timer { get; set; }
		public virtual IList<JsonObject> Results { get; set; }
		public virtual string Err { get; set; }

	}

}