﻿using NUnit.Framework;
using RexConnectClient.Core.Result;
using Weaver.Core.Query;
using Weaver.Exec.RexConnect;

namespace Weaver.Test.WeavExecRexConn {

	/*================================================================================================*/
	[TestFixture]
	public class TRexConnExtensions : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category(Integration)]
		public void ExecuteRequest() {
			var r = new WeaverRequest("1234");
			r.AddQuery("g");
			
			IResponseResult result = r.Execute(RexConnHost, RexConnPort);
			
			CheckResult(result, "1234", 1, 1);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category(Integration)]
		public void ExecuteQuery() {
			var q = new WeaverQuery();
			q.FinalizeQuery("g");

			IResponseResult result = q.Execute(RexConnHost, RexConnPort, "1234");

			CheckResult(result, "1234", 1, 1);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category(Integration)]
		public void ExecuteTx() {
			var tx = new WeaverTransaction();

			var q = new WeaverQuery();
			q.FinalizeQuery("g");
			tx.AddQuery(q);

			q = new WeaverQuery();
			q.FinalizeQuery("x = 10+2");
			tx.AddQuery(q);

			q = new WeaverQuery();
			q.FinalizeQuery("x*x");
			tx.AddQuery(q);

			tx.Finish();

			IResponseResult result = tx.Execute(RexConnHost, RexConnPort, "1234");
			
			CheckResult(result, "1234", 1, 1);
			Assert.AreEqual(144, result.GetTextResultsAt(0).ToInt(0), "Incorrect result value.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category(Integration)]
		public void ExecuteAsSession() {
			var tx = new WeaverTransaction();
			
			var q = new WeaverQuery();
			q.FinalizeQuery("'zach'");
			tx.AddQuery(q);
			
			q = new WeaverQuery();
			q.FinalizeQuery("x = 10+2;");
			tx.AddQuery(q);
			
			q = new WeaverQuery();
			q.FinalizeQuery("x*x");
			tx.AddQuery(q);
			
			tx.Finish();
			
			IResponseResult result = tx.ExecuteAsSession(RexConnHost, RexConnPort, "1234");
			
			CheckResult(result, "1234", 3+3, 1); //include session start/commit/close
			Assert.AreEqual("zach", result.GetTextResultsAt(1).ToString(0), "Incorrect 1,0 result value.");
			Assert.AreEqual(12, result.GetTextResultsAt(2).ToInt(0), "Incorrect 2,0 result value.");
			Assert.AreEqual(144, result.GetTextResultsAt(3).ToInt(0), "Incorrect 3,0 result value.");
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void CheckResult(IResponseResult pResult, string pReqId, int pCmdCount,int pCmd0Results){
			Assert.NotNull(pResult, "Result should be filled.");
			Assert.NotNull(pResult.Response, "Response should be filled.");
			Assert.NotNull(pResult.ResponseJson, "ResponseJson should be filled.");
			Assert.AreEqual(pReqId, pResult.Response.ReqId, "Incorrect Response.ReqId.");
			
			Assert.NotNull(pResult.Response.CmdList, "Response.CmdList should be filled.");
			Assert.AreEqual(pCmdCount, pResult.Response.CmdList.Count, "Incorrect Response.CmdList.Count.");
			
			Assert.NotNull(pResult.Response.CmdList[0].Results,
				"Response.CmdList[0].Results should be filled.");
			Assert.AreEqual(pCmd0Results, pResult.Response.CmdList[0].Results.Count,
				"Incorrect Response.CmdList[0].Results.Count.");
		}

	}

}