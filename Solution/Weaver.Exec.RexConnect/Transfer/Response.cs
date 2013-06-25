using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class Response {

		public virtual string ReqId { get; set; }
		public virtual string SessId { get; set; }
		public virtual long Timer { get; set; }
		public virtual string Err { get; set; }
		public virtual IList<ResponseCmd> CmdList { get; set; }

	}

}