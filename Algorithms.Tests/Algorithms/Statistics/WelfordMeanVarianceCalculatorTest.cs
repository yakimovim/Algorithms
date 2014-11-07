using System;
using EdlinSoftware.Algorithms.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Statistics
{
    [TestClass]
    public class WelfordMeanVarianceCalculatorTest
    {
        private const int Seed = 12142;
        private const long StreamLength = 10000;

        private WelfordMeanVarianceCalculator _calculator;
        private Random _randomNumbersGenerator;

        [TestInitialize]
        public void TestInitialize()
        {
            _calculator = new WelfordMeanVarianceCalculator();
            _randomNumbersGenerator = new Random(Seed);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void InitialValues()
        {
            Assert.AreEqual(0L, _calculator.Count);
            Assert.AreEqual(0.0, _calculator.Mean);
            Assert.AreEqual(0.0, _calculator.Variance);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnOneInput()
        {
            _calculator.Add(1.0);

            Assert.AreEqual(1L, _calculator.Count);
            Assert.AreEqual(1.0, _calculator.Mean);
            Assert.AreEqual(0.0, _calculator.Variance);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnSeveralSameInputs()
        {
            _calculator.Add(1.0);
            _calculator.Add(1.0);
            _calculator.Add(1.0);

            Assert.AreEqual(3L, _calculator.Count);
            Assert.AreEqual(1.0, _calculator.Mean);
            Assert.AreEqual(0.0, _calculator.Variance);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnSeveralDifferentInputs()
        {
            _calculator.Add(1.0);
            _calculator.Add(2.0);
            _calculator.Add(3.0);

            Assert.AreEqual(3L, _calculator.Count);
            Assert.AreEqual(2.0, _calculator.Mean);
            Assert.AreEqual(1.0, _calculator.Variance);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnManyInputsBetween0And1()
        {
            for (int i = 0; i < StreamLength; i++)
            {
                _calculator.Add(_randomNumbersGenerator.NextDouble());
            }

            Assert.AreEqual(StreamLength, _calculator.Count);
            Assert.IsTrue(_calculator.Mean >= 0.0 && _calculator.Mean <= 1.0);
            Assert.IsTrue(_calculator.Variance >= 0.0 && _calculator.Variance <= 1.0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnManyBigInputs()
        {
            for (int i = 0; i < StreamLength; i++)
            {
                _calculator.Add(_randomNumbersGenerator.NextDouble() + 1e9);
            }

            Assert.AreEqual(StreamLength, _calculator.Count);
            Assert.IsTrue(_calculator.Mean >= 1e9 && _calculator.Mean <= 1e9 + 1.0);
            Assert.IsTrue(_calculator.Variance >= 0.0 && _calculator.Variance <= 1.0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void ValuesOnManySmallInputs()
        {
            for (int i = 0; i < StreamLength; i++)
            {
                _calculator.Add(_randomNumbersGenerator.NextDouble() / 1e9);
            }

            Assert.AreEqual(StreamLength, _calculator.Count);
            Assert.IsTrue(_calculator.Mean >= 0.0 && _calculator.Mean <= 1.0 / 1e9);
            Assert.IsTrue(_calculator.Variance >= 0.0 && _calculator.Variance <= 1.0 / 1e9);
        }
    }
}