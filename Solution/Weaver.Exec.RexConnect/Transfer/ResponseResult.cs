using System;
using System.Collections.Generic;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class ResponseResult {

		public Request Request { get; private set; }
		public string RequestJson { get; private set; }

		public Response Response { get; private set; }
		public string ResponseJson { get; private set; }
		public IList<CommandResult> CommandResults { get; private set; }

		public int ExecutionMilliseconds { get; private set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ResponseResult(Request pRequest, string pRequestJson, Response pResponse, 
												string pResponseJson, int pExecutionMilliseconds) {
			Request = pRequest;
			RequestJson = pRequestJson;
			Response = pResponse;
			ResponseJson = pResponseJson;
			ExecutionMilliseconds = pExecutionMilliseconds;

			CommandResults = new List<CommandResult>();

			foreach ( ResponseCmd cmd in Response.CmdList ) {
				CommandResults.Add(new CommandResult(cmd));
			}
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void FillCommandTextResults() {
			int count = Response.CmdList.Count;
			int i = 0;
			string json = (string)ResponseJson.Clone();

			while ( i < count && json != null ) {
				json = BuildTextListResults(json, Response.CmdList[i++]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private static string BuildTextListResults(string pRemainingJson, ResponseCmd pCmd) {
			const string cmdResults = "\"results\":[";
			var list = new List<string>();
			int startI = pRemainingJson.IndexOf(cmdResults);

			if ( startI == -1 ) {
				return null;
			}

			startI += cmdResults.Length;
			string text = pRemainingJson.Substring(startI);

			////

			int i = 0;
			int j = -1;
			int lastIndex = text.Length-1;
			bool inQuote = false;
			int inCurly = 0;
			int inSquare = 1; //looking for a closing square brace

			foreach ( char c in text ) {
				++j;

				switch ( c ) {
					case '"':
						inQuote = !inQuote;
						break;

					case '{':
						inCurly++;
						break;

					case '}':
						inCurly--;
						break;

					case '[':
						inSquare++;
						break;

					case ']':
						inSquare--;
						break;
				}

				if ( j == lastIndex ) {
					throw new Exception("Invalid JSON found in this text: "+pRemainingJson);
				}

				if ( inSquare == 0 ) {
					list.Add(text.Substring(i, j-i).Trim());
					i = j+1;
					break;
				}

				if ( inQuote || inCurly > 0 || inSquare > 0 ) {
					continue;
				}

				if ( c == ',' ) {
					list.Add(text.Substring(i, j-i).Trim());
					i = j+1;
					continue;
				}
			}

			pCmd.TextResults = new TextResultList(list);
			return pRemainingJson.Substring(i);
		}

	}

}