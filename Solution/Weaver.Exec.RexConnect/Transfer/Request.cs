using System.Collections.Generic;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class Request {

		public virtual string ReqId { get; set; }
		public virtual string SessId { get; set; }
		public virtual IList<RequestCmd> CmdList { get; set; }

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual string ToJson() {
			JsConfig.EmitCamelCaseNames = true;
			string json = JsonSerializer.SerializeToString(this);
			JsConfig.EmitCamelCaseNames = false;
			return json;
		}

	}
	
}