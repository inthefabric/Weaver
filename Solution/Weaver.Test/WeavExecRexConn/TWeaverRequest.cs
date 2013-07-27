using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RexConnectClient.Core.Transfer;
using Weaver.Core.Query;
using Weaver.Exec.RexConnect;

namespace Weaver.Test.WeavExecRexConn {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverRequest : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var r = new WeaverRequest();

			Assert.Null(r.ReqId, "ReqId should be null.");
			Assert.Null(r.SessId, "SessId should be null.");
			AssertCmdList(r, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewReq() {
			const string reqId = "myReqId";
			var r = new WeaverRequest(reqId);

			Assert.AreEqual(reqId, r.ReqId, "Incorrect ReqId.");
			Assert.Null(r.SessId, "SessId should be null.");
			AssertCmdList(r, 0);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewReqSess() {
			const string reqId = "myReqId";
			const string sessId = "mySessId";
			var r = new WeaverRequest(reqId, sessId);

			Assert.AreEqual(reqId, r.ReqId, "Incorrect ReqId.");
			Assert.AreEqual(sessId, r.SessId, "Incorrect SessId.");
			AssertCmdList(r, 0);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddQuery() {
			const string script = "g.V('ItemId',P0).outE(P1);";
			var param = new Dictionary<string, IWeaverQueryVal>();
			param.Add("P0", new WeaverQueryVal("id"));
			param.Add("P1", new WeaverQueryVal("uses"));

			var r = new WeaverRequest();
			RequestCmd cmd = r.AddQuery(script, param);

			const string expectScript = script;
			const string expectParamJson = "{\"P0\":\"id\",\"P1\":\"uses\"}";

			AssertCmdList(r, 1);
			AssertCmd(cmd, "query", expectScript, expectParamJson);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddQueryNoParams(bool pNullParams) {
			var param = (pNullParams ? null : new Dictionary<string, IWeaverQueryVal>());

			var r = new WeaverRequest();
			RequestCmd cmd = r.AddQuery("g", param);

			AssertCmdList(r, 1);
			AssertCmd(cmd, "query", "g");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddQueryWeaverScript() {
			const string script = "g.V('ItemId',P0).outE(P1);";
			var param = new Dictionary<string, IWeaverQueryVal>();
			param.Add("P0", new WeaverQueryVal("id"));
			param.Add("P1", new WeaverQueryVal("uses"));

			var mockScr = new Mock<IWeaverScript>();
			mockScr.SetupGet(x => x.Script).Returns(script);
			mockScr.SetupGet(x => x.Params).Returns(param);

			var r = new WeaverRequest();
			RequestCmd cmd = r.AddQuery(mockScr.Object);

			const string expectScript = script;
			const string expectParamJson = "{\"P0\":\"id\",\"P1\":\"uses\"}";

			AssertCmdList(r, 1);
			AssertCmd(cmd, "query", expectScript, expectParamJson);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AddQueryWeaverScriptNoParams(bool pNullParams) {
			var param = (pNullParams ? null : new Dictionary<string, IWeaverQueryVal>());

			var mockScr = new Mock<IWeaverScript>();
			mockScr.SetupGet(x => x.Script).Returns("x");
			mockScr.SetupGet(x => x.Params).Returns(param);

			var r = new WeaverRequest();
			RequestCmd cmd = r.AddQuery(mockScr.Object);

			AssertCmdList(r, 1);
			AssertCmd(cmd, "query", "x");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true, true)]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(false, false)]
		public void AddQueries(bool pAsTransaction, bool pAsSession) {
			var list = new List<IWeaverQuery>();

			var mockScr = new Mock<IWeaverQuery>();
			mockScr.SetupGet(x => x.Script).Returns("x");
			list.Add(mockScr.Object);

			mockScr = new Mock<IWeaverQuery>();
			mockScr.SetupGet(x => x.Script).Returns("y");
			list.Add(mockScr.Object);

			mockScr = new Mock<IWeaverQuery>();
			mockScr.SetupGet(x => x.Script).Returns("z");
			list.Add(mockScr.Object);

			var r = new WeaverRequest();
			IList<RequestCmd> cmd;
			
			 if ( pAsTransaction ) {
				var mockTx = new Mock<IWeaverTransaction>();
				mockTx.SetupGet(x => x.Queries).Returns(list);
				
				cmd = r.AddQueries(mockTx.Object, pAsSession);
			 }
			 else {
				cmd = r.AddQueries(list, pAsSession);
			}

			AssertCmdList(r, (pAsSession ? 6 : 3));
			int i = 0;

			if ( pAsSession ) {
				AssertCmd(cmd[i++], "session", "start");
			}

			AssertCmd(cmd[i++], "query", "x");
			AssertCmd(cmd[i++], "query", "y");
			AssertCmd(cmd[i++], "query", "z");

			if ( pAsSession ) {
				AssertCmd(cmd[i++], "session", "commit");
				AssertCmd(cmd[i++], "session", "close");
			}
		}
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void AssertCmdList(WeaverRequest pReq, int pLen) {
			Assert.NotNull(pReq.CmdList, "CmdList should be filled.");
			Assert.AreEqual(pLen, pReq.CmdList.Count, "Incorrect CmdList.Count.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private void AssertCmd(RequestCmd pCmd, string pCmdText, params string[] pArgs) {
			Assert.AreEqual(pCmdText, pCmd.Cmd, "Incorrect Cmd.");
			Assert.NotNull(pCmd.Args, "Cmd.Args should be filled.");
			Assert.AreEqual(pArgs, pCmd.Args, "Incorrect Args.");
		}

	}

}