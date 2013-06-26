using System.Collections.Generic;
using System.Text.RegularExpressions;
using Weaver.Core.Query;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class RequestCmd {

		public string Cmd { get; set; }
		public IList<string> Args { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RequestCmd(string pCommand, params string[] pArgs) {
			Cmd = pCommand;
			Args = new List<string>(pArgs);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd CreateQueryCommand(string pScript) {
			return new RequestCmd(RexConn.Command.Query.ToString().ToLower(), JsonUnquote(pScript));
		}

		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd CreateQueryCommand(string pScript,
														IDictionary<string, IWeaverQueryVal> pParams) {
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

			return new RequestCmd(RexConn.Command.Query.ToString().ToLower(), q, "{"+p+"}");
		}

		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd CreateSessionCommand(RexConn.SessionAction pAction) {
			return new RequestCmd(RexConn.Command.Session.ToString().ToLower(),
				pAction.ToString().ToLower());
		}

		/*--------------------------------------------------------------------------------------------*/
		public static RequestCmd CreateConfigCommand(RexConn.ConfigSetting pSetting, string pValue) {
			return new RequestCmd(RexConn.Command.Session.ToString().ToLower(),
				pSetting.ToString().ToLower(), pValue);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static string JsonUnquote(string pText) {
			return pText.Replace("\"", "\\\"");
		}

	}

}