using Moq;
using NUnit.Framework;
using Weaver.Core.Exceptions;
using Weaver.Core.Path;
using Weaver.Core.Query;
using Weaver.Core.Steps;
using Weaver.Test.Core.Common;
using Weaver.Test.Core.Common.Vertices;
using Weaver.Test.Core.Utils;

namespace Weaver.Test.Core.Fixtures.Pipe {

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
		[Test]
		public void ToQuery() {
			var q = new WeaverQuery();
			vElem.MockPath.SetupGet(x => x.Query).Returns(q);

			IWeaverQuery result = vElem.ToQuery();

			Assert.AreEqual(q, result, "Incorrect result.");
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void SelfError() {
			var badSelf = new BadSelf();
			WeaverTestUtil.CheckThrows<WeaverPathException>(true, () => badSelf.Dedup());
		}
		
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
		[TestCase("aggregate(_V0)")]
		public void Aggregate(string pExpect) {
			vElem.Aggregate(NewVarAlias());
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("retain(_V0)")]
		public void Retain(string pExpect) {
			vElem.Retain(NewVarAlias());
			VerifyCustom(vElem, pExpect);
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase("except(_V0)")]
		public void Except(string pExpect) {
			vElem.Except(NewVarAlias());
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
		private static IWeaverVarAlias NewVarAlias() {
			return new WeaverVarAlias(new WeaverTransaction());
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
	public class BadSelf : TestVertex<Candy> {
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public BadSelf() {
			var mockPath = new Mock<IWeaverPath>();
			Path = mockPath.Object;
		}

	}

}