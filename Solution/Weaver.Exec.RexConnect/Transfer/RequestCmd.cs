using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class RequestCmd {

		public virtual string Cmd { get; set; }
		public virtual IList<string> Args { get; set; }

	}

}