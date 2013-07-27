using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Query;
using Weaver.Test.Utils;

namespace Weaver.Test.WeavCore.Query {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverTransaction : WeaverTestBase {
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var tx = new WeaverTransaction();
			Assert.NotNull(tx.Queries, "Queries should not be null.");
			Assert.AreEqual(0, tx.Queries.Count, "Incorrect Queries.Count.");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null)]
		[TestCase("_endVar")]
		public void Finish(string pFinalOutput) {
			var q1Params = new Dictionary<string, IWeaverQueryVal>();
			q1Params.Add("_P0", new WeaverQueryVal("first"));
			q1Params.Add("_P1", new WeaverQueryVal("second"));

			var q2Params = new Dictionary<string, IWeaverQueryVal>();
			q2Params.Add("_P0", new WeaverQueryVal("third"));
			q2Params.Add("_P1", new WeaverQueryVal("fourth"));
			q2Params.Add("_P2", new WeaverQueryVal("fifth"));

			var q3Params = new Dictionary<string, IWeaverQueryVal>();
			q3Params.Add("_P0", new WeaverQueryVal("sixth"));
			q3Params.Add("_P1", new WeaverQueryVal("sixth"));
			q3Params.Add("_P2", new WeaverQueryVal("sixth"));
			q3Params.Add("_P3", new WeaverQueryVal("sixth"));
			q3Params.Add("_P4", new WeaverQueryVal("sixth"));
			q3Params.Add("_P5", new WeaverQueryVal("sixth"));
			q3Params.Add("_P6", new WeaverQueryVal("sixth"));
			q3Params.Add("_P7", new WeaverQueryVal("sixth"));
			q3Params.Add("_P8", new WeaverQueryVal("sixth"));
			q3Params.Add("_P9", new WeaverQueryVal("sixth"));
			q3Params.Add("_P10", new WeaverQueryVal("sixth"));
			q3Params.Add("_P11", new WeaverQueryVal("sixth"));
			q3Params.Add("_P12", new WeaverQueryVal("sixth"));
			q3Params.Add("_P13", new WeaverQueryVal("sixth"));

			var mockQ1 = new Mock<IWeaverQuery>();
			mockQ1.SetupGet(x => x.Script).Returns("g.outE(_P0).inV.inE(_P1).outV;");
			mockQ1.SetupGet(x => x.Params).Returns(q1Params);
			
			var mockQ2 = new Mock<IWeaverQuery>();
			mockQ2.SetupGet(x => x.Script).Returns("g.inV.inE(_P0).inV.inE(_P1).inE(_P2);");
			mockQ2.SetupGet(x => x.Params).Returns(q2Params);

			string q3Script = "";
			string exScript = "";

			for ( int i = 0 ; i <= 13 ; ++i ) {
				q3Script += ".inE(_P"+i+")";
				exScript += ".inE(_TP"+(i+5)+")";
			}

			var mockQ3 = new Mock<IWeaverQuery>();
			mockQ3.SetupGet(x => x.Script).Returns("g.inV"+q3Script+";");
			mockQ3.SetupGet(x => x.Params).Returns(q3Params);

			var tx = new WeaverTransaction();
			tx.AddQuery(mockQ1.Object);
			tx.AddQuery(mockQ2.Object);
			tx.AddQuery(mockQ3.Object);

			IWeaverVarAlias finalVar = null;

			if ( pFinalOutput != null ) {
				var mockVar = new Mock<IWeaverVarAlias>();
				mockVar.SetupGet(x => x.Name).Returns(pFinalOutput);
				finalVar = mockVar.Object;
			}

			string expectScript =
				"g.outE(_TP0).inV.inE(_TP1).outV;"+
				"g.inV.inE(_TP2).inV.inE(_TP3).inE(_TP4);"+
				"g.inV"+exScript+";"+
				(pFinalOutput == null ? "" : pFinalOutput+";");

			tx.Finish(finalVar);

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_TP0", q1Params["_P0"]);
			expectParams.Add("_TP1", q1Params["_P1"]);
			expectParams.Add("_TP2", q2Params["_P0"]);
			expectParams.Add("_TP3", q2Params["_P1"]);
			expectParams.Add("_TP4", q2Params["_P2"]);
			expectParams.Add("_TP5", q3Params["_P0"]);
			expectParams.Add("_TP6", q3Params["_P1"]);
			expectParams.Add("_TP7", q3Params["_P2"]);
			expectParams.Add("_TP8", q3Params["_P3"]);
			expectParams.Add("_TP9", q3Params["_P4"]);
			expectParams.Add("_TP10", q3Params["_P5"]);
			expectParams.Add("_TP11", q3Params["_P6"]);
			expectParams.Add("_TP12", q3Params["_P7"]);
			expectParams.Add("_TP13", q3Params["_P8"]);
			expectParams.Add("_TP14", q3Params["_P9"]);
			expectParams.Add("_TP15", q3Params["_P10"]);
			expectParams.Add("_TP16", q3Params["_P11"]);
			expectParams.Add("_TP17", q3Params["_P12"]);
			expectParams.Add("_TP18", q3Params["_P13"]);

			Assert.AreEqual(expectScript, tx.Script, "Incorrect Script.");
			Assert.AreEqual(expectParams, tx.Params, "Incorrect Params.");
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(99)]
		public void AddQuery(int pCount) {
			var tx = new WeaverTransaction();
			var list = new List<IWeaverQuery>();
			
			for ( int i = 0 ; i < pCount ; ++i ) {
				var q = new WeaverQuery();
				list.Add(q);
				tx.AddQuery(q);
			}
			
			Assert.AreEqual(pCount, tx.Queries.Count, "Incorrect Queries.Count.");
			Assert.AreEqual(list, tx.Queries, "Incorrect Query list.");
		}
		
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
			tx.Finish();
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
			tx.Finish();
			WeaverTestUtil.CheckThrows<WeaverException>(true,
				() => tx.Finish());
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		[Category("Integration")]
		public void CustomVarName() {
			var tx = new WeaverTransaction();
			const string custom = "_Custom";
			IWeaverVarAlias alias;

			var q = new WeaverQuery();
			q.FinalizeQuery("g.V[0]");
			var q2 = WeaverQuery.StoreResultAsVar(custom, q, out alias);
			tx.AddQuery(q2);
			tx.Finish(alias);

			const string expect = custom+"=g.V[0];"+custom+";";
			Assert.AreEqual(expect, tx.Script, "Incorrect tx.Script.");
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private IWeaverQuery GetBasicQuery() {
			var q = new Mock<IWeaverQuery>();
			q.SetupGet(x => x.Script).Returns("g.V;");
			q.SetupGet(x => x.Params).Returns(new Dictionary<string, IWeaverQueryVal>());
			return q.Object;
		}

	}

}