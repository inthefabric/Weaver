using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServiceStack.Text;
using Weaver.Exec.RexConnect.Transfer;

namespace Weaver.Exec.RexConnect {

	/*================================================================================================*/
	public class RexConnDataAccess : IRexConnDataAccess {

		private static int TcpCount;

		public IRexConnContext ApiCtx { get; private set; }

		public Request Request { get; private set; }
		public string RequestJson { get; private set; }

		public Response Response { get; private set; }
		public string ResponseJson { get; private set; }
		public int ExecutionMilliseconds { get; private set; }

		private Exception vUnhandledException;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public RexConnDataAccess(IRexConnContext pContext, Request pRequest) {
			ApiCtx = pContext;
			Request = pRequest;
		}

		/*--------------------------------------------------------------------------------------------*/
		public virtual void Execute() {
			var sw = Stopwatch.StartNew();

			try {
				++TcpCount;
				RequestJson = Request.ToJson();
				ApiCtx.Log("Debug", "Request", RequestJson);

				ResponseJson = GetRawResult(RequestJson);
				Response = JsonSerializer.DeserializeFromString<Response>(ResponseJson);

				if ( Response == null ) {
					throw new Exception("Response is null.");
				}

				Response.RawJson = ResponseJson;

				if ( Response.Err != null ) {
					throw new Exception("Response has an error.");
				}
			}
			catch ( WebException we ) {
				vUnhandledException = we;

				Response = new Response();
				Response.Err = we+"";
				Response.CmdList = new List<ResponseCmd>();

				Stream s = (we.Response == null ? null : we.Response.GetResponseStream());

				if ( s != null ) {
					var sr = new StreamReader(s);
					ApiCtx.Log("Error", "Gremlin", sr.ReadToEnd());
				}
			}
			catch ( Exception e ) {
				vUnhandledException = e;
				ApiCtx.Log("Error", "Unhandled", "Raw result: "+ResponseJson);

				Response = new Response();
				Response.Err = e+"";
				Response.CmdList = new List<ResponseCmd>();
			}

			--TcpCount;
			ExecutionMilliseconds = (int)sw.ElapsedMilliseconds;
			LogAction();

			if ( vUnhandledException != null ) {
				vUnhandledException = new Exception("Unhandled exception:"+
					"\nRequestJson = "+RequestJson+
					"\nResponseJson = "+ResponseJson, vUnhandledException);
				throw vUnhandledException;
			}

			for ( int i = 0 ; i < Response.CmdList.Count ; ++i ) {
				ResponseCmd rc = Response.CmdList[i];

				if ( rc.Err != null ) {
					ApiCtx.Log("Warn", "Data", "Response.CmdList["+i+"] error: "+rc.Err);
				}
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		protected virtual string GetRawResult(string pReqJson) {
			TcpClient tcp = new TcpClient(ApiCtx.HostName, ApiCtx.Port);
			tcp.SendBufferSize = tcp.ReceiveBufferSize = 1<<16;

			byte[] dataLen = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(pReqJson.Length));
			byte[] data = Encoding.ASCII.GetBytes(pReqJson);
			
			//stream the request's string length, then the string itself

			NetworkStream stream = tcp.GetStream();
			stream.Write(dataLen, 0, dataLen.Length);
			stream.Write(data, 0, data.Length);

			//Get string length from the first four bytes

			data = new byte[4];
			stream.Read(data, 0, data.Length);
			Array.Reverse(data);
			int respLen = BitConverter.ToInt32(data, 0);

			//Get response string using the string length

			var sb = new StringBuilder(respLen);

			while ( sb.Length < respLen ) {
				data = new byte[respLen];
				int bytes = stream.Read(data, 0, data.Length);

				if ( bytes == 0 ) {
					throw new Exception("Empty read from NetworkStream. "+
						"Expected "+respLen+" chars, received "+sb.Length+" total.");
				}

				sb.Append(Encoding.ASCII.GetString(data, 0, bytes));
			}

			string result = sb.ToString();
			ApiCtx.Log("Debug", "Result", result);
			return result;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void LogAction() {
			//DBv1: 
			//	TotalMs, QueryMs, Timestamp, TcpCount, QueryChars

			const string name = "DBv1";
			const string x = " | ";

			string v1 =
				ExecutionMilliseconds +x+
				Response.Timer +x+
				DateTime.UtcNow.Ticks +x+
				TcpCount +x+
				RequestJson.Length;

			if ( vUnhandledException == null ) {
				ApiCtx.Log("Info", name, v1);
			}
			else {
				ApiCtx.Log("Error", name, v1, vUnhandledException);
			}
		}

	}

}