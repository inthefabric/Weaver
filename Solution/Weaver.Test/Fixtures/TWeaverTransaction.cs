using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Interfaces;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTransaction : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(WeaverTransaction.ConclusionType.Success, "SUCCESS")]
		[TestCase(WeaverTransaction.ConclusionType.Failure, "FAILURE")]
		public void Finish(WeaverTransaction.ConclusionType pConclude, string pConcludeStr) {
			var q1Params = new Dictionary<string, string>();
			q1Params.Add("_P0", "first");
			q1Params.Add("_P1", "second");
			
			var q2Params = new Dictionary<string, string>();
			q2Params.Add("_P0", "third");
			q2Params.Add("_P1", "fourth");
			q2Params.Add("_P2", "fifth");
			
			var q3Params = new Dictionary<string, string>();
			q3Params.Add("_P0", "sixth");

			var mockQ1 = new Mock<IWeaverQuery>();
			mockQ1.SetupGet(x => x.Script).Returns("g.outE(_P0).inV.inE(_P1).outV;");
			mockQ1.SetupGet(x => x.Params).Returns(q1Params);
			
			var mockQ2 = new Mock<IWeaverQuery>();
			mockQ2.SetupGet(x => x.Script).Returns("g.inV.inE(_P0).inV.inE(_P1).inE(_P2);");
			mockQ2.SetupGet(x => x.Params).Returns(q2Params);
			
			var mockQ3 = new Mock<IWeaverQuery>();
			mockQ3.SetupGet(x => x.Script).Returns("g.inV.inE(_P0);");
			mockQ3.SetupGet(x => x.Params).Returns(q3Params);

			var tx = new WeaverTransaction();
			tx.AddQuery(mockQ1.Object);
			tx.AddQuery(mockQ2.Object);
			tx.AddQuery(mockQ3.Object);
			tx.Finish(pConclude);

			string expectScript = "g.startTransaction();"+
				"g.outE(_TP0).inV.inE(_TP1).outV;"+
				"g.inV.inE(_TP2).inV.inE(_TP3).inE(_TP4);"+
				"g.inV.inE(_TP5);"+
				"g.stopTransaction(TransactionalGraph.Conclusion."+pConcludeStr+");";

			var expectParams = new Dictionary<string, string>();
			expectParams.Add("_TP0", q1Params["_P0"]);
			expectParams.Add("_TP1", q1Params["_P1"]);
			expectParams.Add("_TP2", q2Params["_P0"]);
			expectParams.Add("_TP3", q2Params["_P1"]);
			expectParams.Add("_TP4", q2Params["_P2"]);
			expectParams.Add("_TP5", q3Params["_P0"]);

			Assert.AreEqual(expectScript, tx.Script, "Incorrect Script.");
			Assert.AreEqual(expectParams, tx.Params, "Incorrect Params.");
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FinishWithVar() {
			const string name = "_var0";
		
			var mockVar = new Mock<IWeaverVarAlias>();
			mockVar.SetupGet(x => x.Name).Returns(name);
			
			var mockQ1 = new Mock<IWeaverQuery>();
			mockQ1.SetupGet(x => x.Script).Returns("g.V;");
			mockQ1.SetupGet(x => x.Params).Returns(new Dictionary<string,string>());
			
			var tx = new WeaverTransaction();
			tx.AddQuery(mockQ1.Object);
			tx.Finish(WeaverTransaction.ConclusionType.Success, mockVar.Object);
			
			const string expectScript = "g.startTransaction();"+
				"g.V;"+
				"g.stopTransaction(TransactionalGraph.Conclusion.SUCCESS);"+
				name+";";
			
			Assert.AreEqual(expectScript, tx.Script, "Incorrect Script.");
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddQueryNull() {
			var tx = new WeaverTransaction();
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => tx.AddQuery(null));
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddQueryFinished() {
			IWeaverQuery wq = GetBasicQuery();

			var tx = new WeaverTransaction();
			tx.AddQuery(wq);
			tx.Finish(WeaverTransaction.ConclusionType.Success);
			WeaverTestUtil.CheckThrows<WeaverException>(true, () => tx.AddQuery(wq));
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void GetNextVarName() {
			var tx = new WeaverTransaction();

			for ( int i = 0 ; i < 101 ; ++i ) {
				string name = tx.GetNextVarName();
				Assert.AreEqual("_V"+i, name, "Incorrect name at index "+i+".");
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FinishFinished() {
			IWeaverQuery wq = GetBasicQuery();

			var tx = new WeaverTransaction();
			tx.AddQuery(wq);
			tx.Finish(WeaverTransaction.ConclusionType.Success);
			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => tx.Finish(WeaverTransaction.ConclusionType.Success));
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void FinishNoQueries() {
			var tx = new WeaverTransaction();
			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => tx.Finish(WeaverTransaction.ConclusionType.Success));
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IWeaverQuery GetBasicQuery() {
			var q = new Mock<IWeaverQuery>();
			q.SetupGet(x => x.Script).Returns("g.V;");
			q.SetupGet(x => x.Params).Returns(new Dictionary<string, string>());
			return q.Object;
		}

	}

}