﻿using Moq;
using NUnit.Framework;
using Weaver.Core.Path;
using Weaver.Core.Pipe;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Core.Steps.Statements;
using Weaver.Test.Common;
using Weaver.Test.Common.Vertices;
using Weaver.Core;
using System.Linq.Expressions;
using System;

namespace Weaver.Test.WeavCore.Pipe {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverPathPipe : WeaverTestBase {

		private TestElement vElem;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[SetUp]
		protected override void SetUp() {
			base.SetUp();
			vElem = new TestElement();
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("count()")]
		public void Count(string pExpect) {
			vElem.Count();
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("iterate()")]
		public void Iterate(string pExpect) {
			vElem.Iterate();
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("remove()")]
		public void Remove(string pExpect) {
			vElem.Remove();
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToQuery() {
			var q = new WeaverQuery();
			vElem.MockPath.SetupGet(x => x.Query).Returns(q);

			IWeaverQuery result = vElem.ToQuery();

			Assert.AreEqual(q, result, "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToQueryAsVar() {
			IWeaverVarAlias alias;

			var q = new WeaverQuery();
			vElem.MockPath.SetupGet(x => x.Query).Returns(q);
			vElem.MockPath.Setup(x => x.BuildParameterizedScript()).Returns("g.v(4)");

			IWeaverQuery result = vElem.ToQueryAsVar("test", out alias);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual("test=g.v(4);", result.Script, "Incorrect Script.");
			Assert.NotNull(alias, "Alias should be filled.");
			Assert.AreEqual("test", alias.Name, "Incorrect Alias.Name.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void ToQueryAsVarT() {
			IWeaverVarAlias<TestElement> alias;

			var q = new WeaverQuery();
			vElem.MockPath.SetupGet(x => x.Query).Returns(q);
			vElem.MockPath.Setup(x => x.BuildParameterizedScript()).Returns("g.v(4)");

			IWeaverQuery result = vElem.ToQueryAsVar("test", out alias);

			Assert.NotNull(result, "Result should be filled.");
			Assert.AreEqual("test=g.v(4);", result.Script, "Incorrect Script.");
			Assert.NotNull(alias, "Alias should be filled.");
			Assert.AreEqual("test", alias.Name, "Incorrect Alias.Name.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void As() {
			IWeaverStepAs<TestElement> alias;
			TestElement result = vElem.As(out alias);

			Assert.AreEqual(vElem, result, "Incorrect result.");
			Assert.NotNull(alias, "Alias should be filled.");
			Assert.AreEqual(vElem, alias.Item, "Incorrect Alias.Item.");
			VerifyFirstPathItem<WeaverStepAs<TestElement>>(vElem);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Back() {
			var mockPer = new Mock<Person>();
			mockPer.SetupGet(x => x.PathIndex).Returns(4);

			var mockAlias = new Mock<IWeaverStepAs<Person>>();
			mockAlias.SetupGet(x => x.Item).Returns(mockPer.Object);

			Person result = vElem.Back(mockAlias.Object);

			Assert.AreEqual(mockPer.Object, result, "Incorrect result.");
			VerifyFirstPathItem<WeaverStepBack<Person>>(vElem);
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Has() {
			TestElement result = vElem.Has(x => x.Value, WeaverStepHasOp.EqualTo, 1);

			Assert.AreEqual(vElem, result, "Incorrect result.");
			vElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepHas<TestElement>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasProp() {
			TestElement result = vElem.Has(x => x.Value);

			Assert.AreEqual(vElem, result, "Incorrect result.");
			vElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepHas<TestElement>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasNot() {
			TestElement result = vElem.HasNot(x => x.Value, WeaverStepHasOp.EqualTo, 1);

			Assert.AreEqual(vElem, result, "Incorrect result.");
			vElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepHas<TestElement>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void HasNotProp() {
			TestElement result = vElem.HasNot(x => x.Value);

			Assert.AreEqual(vElem, result, "Incorrect result.");
			vElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepHas<TestElement>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void SideEffect() {
			var mockStat = new Mock<IWeaverStatement<TestElement>>();
			TestElement result = vElem.SideEffect(new [] { mockStat.Object });

			Assert.AreEqual(vElem, result, "Incorrect result.");

			vElem.MockPath
				.Verify(x => x.AddItem(It.IsAny<WeaverStepSideEffect<TestElement>>()), Times.Once());
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Property() {
			IWeaverPathPipeEnd result = vElem.Property(x => x.Value);

			Assert.NotNull((result as WeaverStepProp<TestElement>), "Incorrect result.");
			vElem.MockPath.Verify(x => x.AddItem(It.IsAny<WeaverStepProp<TestElement>>()),Times.Once());
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AsColumnObject(bool pGetAlias) {
			const string label = "test";
			IWeaverStepAsColumn<TestElement> alias;
			TestElement result;
			
			if ( pGetAlias ) {
				result = vElem.AsColumn(label, out alias);
				Assert.NotNull(alias, "Alias should be filled.");
				Assert.AreEqual(label, alias.Label, "Incorrect Alias.Label.");
				Assert.AreEqual(vElem, alias.Item, "Incorrect Alias.Item.");
				Assert.Null(alias.PropName, "PropName should be null.");
			}
			else {
				result = vElem.AsColumn(label);
			}
			
			Assert.AreEqual(vElem, result, "Incorrect result.");
			VerifyFirstPathItem<WeaverStepAs<TestElement>>(vElem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(true)]
		[TestCase(false)]
		public void AsColumnProperty(bool pGetAlias) {
			const string label = "test";
			IWeaverStepAsColumn<TestElement> alias;
			TestElement result;
			
			Expression<Func<TestElement, object>> propExp = ((TestElement x) => x.Value);
			
			var mockCfg = new Mock<IWeaverConfig>();
			mockCfg.Setup(x => x.GetPropertyDbName(propExp)).Returns("Value");
			
			vElem.MockPath.SetupGet(x => x.Config).Returns(mockCfg.Object);
			
			if ( pGetAlias ) {
				result = vElem.AsColumn(label, propExp, out alias);
				Assert.NotNull(alias, "Alias should be filled.");
				Assert.AreEqual(label, alias.Label, "Incorrect Alias.Label.");
				Assert.AreEqual(vElem, alias.Item, "Incorrect Alias.Item.");
				Assert.AreEqual("Value", alias.PropName, "Incorrect Alias.PropName.");
			}
			else {
				result = vElem.AsColumn(label, propExp);
			}
			
			Assert.AreEqual(vElem, result, "Incorrect result.");
			VerifyFirstPathItem<WeaverStepAs<TestElement>>(vElem);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void Table() {
			IWeaverVarAlias alias;
			const int pathLen = 99;
			vElem.MockPath.SetupGet(x => x.Length).Returns(pathLen);
			Console.WriteLine("PATH: "+vElem.Path);
			Console.WriteLine("LEN: "+vElem.Path.Length);
			
			TestElement result = vElem.Table(out alias);
			
			Assert.AreEqual(vElem, result, "Incorrect result.");
			Assert.NotNull(alias, "Alias should be filled.");
			Assert.AreEqual("_TABLE"+pathLen,  alias.Name, "Incorrect Alias.Name.");
			VerifyFirstPathItem<WeaverStepTable>(vElem);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase(null, "next()")]
		[TestCase(1, "next(1)")]
		[TestCase(99, "next(99)")]
		public void Next(int? pCount, string pExpect) {
			vElem.Next(pCount);
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("x", "aggregate(x)")]
		public void Aggregate(string pVarName, string pExpect) {
			vElem.Aggregate(NewVarAlias(pVarName));
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("y", "retain(y)")]
		public void Retain(string pVarName, string pExpect) {
			vElem.Retain(NewVarAlias(pVarName));
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("z", "except(z)")]
		public void Except(string pVarName, string pExpect) {
			vElem.Except(NewVarAlias(pVarName));
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("dedup")]
		public void Dedup(string pExpect) {
			vElem.Dedup();
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, 10, "[0..10]")]
		[TestCase(999, 1234, "[999..1234]")]
		public void Limit(int pStart, int pEnd, string pExpect) {
			vElem.Limit(pStart, pEnd);
			VerifyCustom(vElem, pExpect, true);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(0, "[0]")]
		[TestCase(999, "[999]")]
		public void AtIndex(int pIndex, string pExpect) {
			vElem.AtIndex(pIndex);
			VerifyCustom(vElem, pExpect, true);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[TestCase("test", true)]
		[TestCase("test", false)]
		public void CustomStep(string pScript, bool pSkipDot) {
			vElem.CustomStep(pScript, pSkipDot);
			VerifyCustom(vElem, pScript, pSkipDot);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private static IWeaverVarAlias NewVarAlias(string pName) {
			return new WeaverVarAlias(pName);
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private static void VerifyCustom(TestElement pElem, string pScript, bool pSkipDot=false){
			Assert.AreEqual(1, pElem.PathItems.Count, "Incorrect PathItems.Count.");
			WeaverStepCustom cust = VerifyFirstPathItem<WeaverStepCustom>(pElem);
			Assert.AreEqual(pScript, cust.BuildParameterizedString(), "Incorrect Script.");
			Assert.AreEqual(pSkipDot, cust.SkipDotPrefix, "Incorrect SkipDotPrefix.");
		}

		/*--------------------------------------------------------------------------------------------*/
		private static T VerifyFirstPathItem<T>(TestElement pElem) where T : class {
			Assert.AreEqual(1, pElem.PathItems.Count, "Incorrect PathItems.Count.");
			IWeaverPathItem item = pElem.PathItems[0];
			T itemT = (item as T);
			Assert.NotNull(itemT, "Incorrect item type: "+item.GetType().Name);
			return itemT;
		}

	}


	/*================================================================================================*/
	public class BadSelf : TestVertex {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BadSelf() {
			var mockPath = new Mock<IWeaverPath>();
			Path = mockPath.Object;
		}

	}

}