using System;
using System.Collections.Generic;
using ServiceStack.Text;

namespace Weaver.Exec.RexConnect.Transfer {

	/*================================================================================================*/
	public class ResponseResult : IResponseResult {

		public IRexConnContext Context { get; protected set; }

		public Request Request { get; protected set; }
		public string RequestJson { get; protected set; }

		public Response Response { get; protected set; }
		public string ResponseJson { get; protected set; }
		public IList<CommandResult> CommandResults { get; protected set; }

		public bool IsError { get; protected set; }
		public int ExecutionMilliseconds { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void SetRequest(IRexConnContext pContext, Request pRequest) {
			Context = pContext;
			Request = pRequest;
			RequestJson = JsonSerializer.SerializeToString(pRequest);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetResponseJson(string pResponseJson) {
			ResponseJson = pResponseJson;
			Response = JsonSerializer.DeserializeFromString<Response>(ResponseJson);

			if ( Response == null ) {
				throw new Exception("Response is null.");
			}

			if ( Response.Err != null ) {
				throw new Exception("Response has an error.");
			}

			CommandResults = new List<CommandResult>();

			for ( int i = 0 ; i < Response.CmdList.Count ; ++i ) {
				ResponseCmd rc = Response.CmdList[i];
				CommandResults.Add(new CommandResult(rc));

				if ( rc.Err != null ) {
					Context.Log("Warn", "Data", "Response.CmdList["+i+"] error: "+rc.Err);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetErrorResponse(string pErr) {
			IsError = true;

			Response = new Response();
			Response.Err = pErr;
			Response.CmdList = new List<ResponseCmd>();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual void FillCommandTextResults() {
			int count = CommandResults.Count;
			int i = 0;
			string json = (string)ResponseJson.Clone();

			while ( i < count && json != null ) {
				json = BuildTextListResults(json, CommandResults[i++]);
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected static string BuildTextListResults(string pRemainingJson, CommandResult pCmdRes) {
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

			pCmdRes.TextResults = new TextResultList(list);
			return pRemainingJson.Substring(i);
		}

	}

}