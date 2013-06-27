using NUnit.Framework;
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
		public void ExecuteQuery() {
			var q = new WeaverQuery();
			q.FinalizeQuery("g");

			IResponseResult result = q.Execute(RexConnHost, RexConnPort, "1234");

			Assert.NotNull(result, "Result should be filled.");
			Assert.NotNull(result.Response, "Response should be filled.");
			Assert.NotNull(result.ResponseJson, "ResponseJson should be filled.");
			Assert.AreEqual("1234", result.Response.ReqId, "Incorrect Response.ReqId.");

			Assert.NotNull(result.Response.CmdList, "Response.CmdList should be filled.");
			Assert.AreEqual(1, result.Response.CmdList.Count, "Incorrect Response.CmdList.Count.");

			Assert.NotNull(result.Response.CmdList[0].Results,
				"Response.CmdList[0].Results should be filled.");
			Assert.AreEqual(1, result.Response.CmdList[0].Results.Count,
				"Incorrect Response.CmdList[0].Results.Count.");
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

			Assert.NotNull(result, "Result should be filled.");
			Assert.NotNull(result.Response, "Response should be filled.");
			Assert.NotNull(result.ResponseJson, "ResponseJson should be filled.");
			Assert.AreEqual("1234", result.Response.ReqId, "Incorrect Response.ReqId.");

			Assert.NotNull(result.Response.CmdList, "Response.CmdList should be filled.");
			Assert.AreEqual(1, result.Response.CmdList.Count, "Incorrect Response.CmdList.Count.");

			Assert.NotNull(result.Response.CmdList[0].Results,
				"Response.CmdList[0].Results should be filled.");
			Assert.AreEqual(1, result.Response.CmdList[0].Results.Count,
				"Incorrect Response.CmdList[0].Results.Count.");

			Assert.AreEqual(144, result.GetTextResultsAt(0).ToInt(0), "Incorrect result value.");
		}

	}

}