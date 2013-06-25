using System.Collections.Generic;
using System.Text.RegularExpressions;
using Weaver.Core.Query;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public class RexConn {

		public enum Command {
			Session = 1,
			Query,
			Config
		}

		public enum SessionAction {
			Start = 1,
			Close,
			Commit,
			Rollback
		}

		public enum ConfigSetting {
			Debug = 1,
			Pretty
		}

		public enum GraphElementType {
			Unknown,
			Vertex,
			Edge,
			Error
		}

		public static bool SendRequestsInDebugMode;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static Request BuildRequest(string pRequestId) {
			var req = new Request();
			req.ReqId = pRequestId;
			req.CmdList = new List<RequestCmd>();

			if ( SendRequestsInDebugMode ) {
				var debugCmd = new RequestCmd();
				debugCmd.Cmd = Command.Config.ToString().ToLower();
				debugCmd.Args = new List<string>(new[] { ConfigSetting.Debug.ToString().ToLower(), "1" });
				req.CmdList.Add(debugCmd);
			}

			return req;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Request BuildRequest(string pRequestId, string pScript,
														IDictionary<string, IWeaverQueryVal> pParams) {
			Request req = BuildRequest(pRequestId);
			req.CmdList.Add(BuildQueryCommand(pScript, pParams));
			return req;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Request BuildRequest(string pRequestId, string pScript) {
			return BuildRequest(pRequestId, pScript, null);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Request BuildRequest(string pRequestId, IWeaverScript pWeaverScript) {
			return BuildRequest(pRequestId, pWeaverScript.Script, pWeaverScript.Params);
		}

		/*--------------------------------------------------------------------------------------------*/
		public static Request BuildRequest(string pRequestId, IList<IWeaverScript> pWeaverScripts,
																			bool pAsSession=true) {
			Request req = BuildRequest(pRequestId);

			if ( pAsSession ) {
				req.CmdList.Add(BuildSessionAction(SessionAction.Start));
			}

			foreach ( IWeaverScript ws in pWeaverScripts ) {
				req.CmdList.Add(BuildQueryCommand(ws.Script, ws.Params));
			}

			if ( pAsSession ) {
				req.CmdList.Add(BuildSessionAction(SessionAction.Commit));
				req.CmdList.Add(BuildSessionAction(SessionAction.Close));
			}

			return req;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd BuildQueryCommand(string pScript) {
			var cmd = new RequestCmd();
			cmd.Cmd = Command.Query.ToString().ToLower();
			cmd.Args = new List<string>(new [] { JsonUnquote(pScript) });
			return cmd;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd BuildQueryCommand(string pScript,
														IDictionary<string, IWeaverQueryVal> pParams) {
			var cmd = new RequestCmd();
			cmd.Cmd = Command.Query.ToString().ToLower();

			const string end = @"(?=$|[^\d])";
			string q = JsonUnquote(pScript);
			string p = "";

			foreach ( string key in pParams.Keys ) {
				p += (p.Length > 0 ? "," : "")+"\""+JsonUnquote(key)+"\":";

				IWeaverQueryVal qv = pParams[key];

				if ( qv.IsString ) {
					p += "\""+JsonUnquote(qv.FixedText)+"\"";
					continue;
				}

				p += qv.FixedText;

				//Explicitly cast certain parameter types
				//See: https://github.com/tinkerpop/rexster/issues/295

				if ( qv.Original is int ) {
					q = Regex.Replace(q, key+end, key+".toInteger()");
				}
				else if ( qv.Original is byte ) {
					q = Regex.Replace(q, key+end, key+".byteValue()");
				}
				else if ( qv.Original is float ) {
					q = Regex.Replace(q, key+end, key+".toFloat()");
				}
			}

			cmd.Args = new List<string>(new[] { q, "{"+p+"}" });
			return cmd;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd BuildSessionAction(SessionAction pAction) {
			var cmd = new RequestCmd();
			cmd.Cmd = Command.Session.ToString().ToLower();
			cmd.Args = new List<string>(new[] { pAction.ToString().ToLower() });
			return cmd;
		}

		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd BuildConfigSetting(ConfigSetting pSetting, string pValue) {
			var cmd = new RequestCmd();
			cmd.Cmd = Command.Config.ToString().ToLower();
			cmd.Args = new List<string>(new[] { pSetting.ToString().ToLower(), pValue });
			return cmd;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string JsonUnquote(string pText) {
			return pText.Replace("\"", "\\\"");
		}

	}

}