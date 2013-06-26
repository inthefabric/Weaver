using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Text;
using Weaver.Exec.RexConnect.Result.Strings;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect.Result {

	/*================================================================================================*/
	public class ResponseResult : IResponseResult {

		public IRexConnContext Context { get; protected set; }

		public Request Request { get; protected set; }
		public string RequestJson { get; protected set; }
		public Response Response { get; protected set; }
		public string ResponseJson { get; protected set; }

		public bool IsError { get; protected set; }
		public int ExecutionMilliseconds { get; set; }

		private IList<IList<IGraphElement>> vElementResults;
		private IList<ITextResultList> vTextResults;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public ResponseResult(IRexConnContext pContext) {
			Context = pContext;
			Request = pContext.Request;
			RequestJson = JsonSerializer.SerializeToString(Request);
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetResponseJson(string pResponseJson) {
			VerifyNoResponse();
			ResponseJson = pResponseJson;
			Response = JsonSerializer.DeserializeFromString<Response>(ResponseJson);

			if ( Response == null ) {
				throw new Exception("Response is null.");
			}

			if ( Response.Err != null ) {
				throw new Exception("Response has an error: "+Response.Err);
			}

			for ( int i = 0 ; i < Response.CmdList.Count ; ++i ) {
				ResponseCmd rc = Response.CmdList[i];

				if ( rc.Err != null ) {
					Context.Log("Warn", "Data", "Response.CmdList["+i+"] error: "+rc.Err);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void SetResponseError(string pErr) {
			VerifyNoResponse();
			IsError = true;

			Response = new Response();
			Response.Err = pErr;
			Response.CmdList = new List<ResponseCmd>();
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private void VerifyNoResponse() {
			if ( Response != null ) {
				throw new Exception("Response is already set.");
			}
		}

		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IList<IGraphElement>> GetGraphElements() {
			if ( vElementResults != null ) {
				return vElementResults;
			}

			vElementResults = new List<IList<IGraphElement>>();

			foreach ( ResponseCmd cmd in Response.CmdList ) {
				vElementResults.Add(
					cmd.Results.Select(GraphElement.Build).Cast<IGraphElement>().ToList()
				);
			}

			return vElementResults;
		}
		
		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<IGraphElement> GetGraphElementsAt(int pCommandIndex) {
			GetGraphElements();
			return vElementResults[pCommandIndex];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public virtual IList<ITextResultList> GetTextResults() {
			if ( vTextResults != null ) {
				return vTextResults;
			}

			StringsResponse sr = JsonSerializer.DeserializeFromString<StringsResponse>(ResponseJson);
			vTextResults = new List<ITextResultList>();

			foreach ( StringsResponseCmd src in sr.CmdList ) {
				vTextResults.Add(new TextResultList(src.Results));
			}

			return vTextResults;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual ITextResultList GetTextResultAt(int pCommandIndex) {
			GetTextResults();
			return vTextResults[pCommandIndex];
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------* /
		public virtual void FillCommandTextResults() {
			int count = CommandResults.Count;
			int i = 0;
			string json = (string)ResponseJson.Clone();

			while ( i < count && json != null ) {
				json = BuildTextListResults(json, CommandResults[i++]);
			}
		}

		/*--------------------------------------------------------------------------------------------* /
		protected static string BuildTextListResults(string pRemainingJson, ResponseCmdResult pCmdRes) {
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
		}*/

	}

}