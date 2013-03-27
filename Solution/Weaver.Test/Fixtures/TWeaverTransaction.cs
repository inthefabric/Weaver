﻿using System.Collections.Generic;
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

			IWeaverVarAlias finalVar = null;

			if ( pFinalOutput != null ) {
				var mockVar = new Mock<IWeaverVarAlias>();
				mockVar.SetupGet(x => x.Name).Returns(pFinalOutput);
				finalVar = mockVar.Object;
			}

			string expectScript =
				"g.outE(_TP0).inV.inE(_TP1).outV;"+
				"g.inV.inE(_TP2).inV.inE(_TP3).inE(_TP4);"+
				"g.inV.inE(_TP5);"+
				(pFinalOutput == null ? "" : pFinalOutput+";");

			tx.Finish(finalVar);

			var expectParams = new Dictionary<string, IWeaverQueryVal>();
			expectParams.Add("_TP0", q1Params["_P0"]);
			expectParams.Add("_TP1", q1Params["_P1"]);
			expectParams.Add("_TP2", q2Params["_P0"]);
			expectParams.Add("_TP3", q2Params["_P1"]);
			expectParams.Add("_TP4", q2Params["_P2"]);
			expectParams.Add("_TP5", q3Params["_P0"]);

			Assert.AreEqual(expectScript, tx.Script, "Incorrect Script.");
			Assert.AreEqual(expectParams, tx.Params, "Incorrect Params.");
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
		private IWeaverQuery GetBasicQuery() {
			var q = new Mock<IWeaverQuery>();
			q.SetupGet(x => x.Script).Returns("g.V;");
			q.SetupGet(x => x.Params).Returns(new Dictionary<string, IWeaverQueryVal>());
			return q.Object;
		}

	}

}