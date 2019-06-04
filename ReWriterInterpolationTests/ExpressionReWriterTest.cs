using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using ReWriteInterpolation;
using ReWriteInterpolation.Models;

namespace Tests
{
    // ReSharper disable  ReturnValueOfPureMethodIsNotUsed
    // ReSharper disable  IdentifierTypo
    // ReSharper disable  StringLiteralTypo
    public class ExpressionReWriterTests
    {
        private BotDbContext _context { get; set; }

        
        [OneTimeSetUp]
        public void Setup()
        {
            _context = new BotDbContext();
            _context.Database.EnsureCreated();
            var piter = new Human("Piter", 19);
            _context.Add(piter);
            _context.SaveChanges();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public void OrderBy__OrderByNotInterpolationString__Correct()
        {
            var human = _context.Humans
                .Select(x => new
                {
                    interpolation = $" Name:{x.Name} Surname:{x.SurName} Age {x.Age}.",
                    x.Name
                })
                .OrderBy(x => x.Name);
            Assert.DoesNotThrow(() => human.ToList());
        }

        [Test]
        public void OrderBy___OrderByInterpolationString__InvalidOperationException()
        {
            var s = "sca";
            var human = _context.Humans
                .Select(x => $" Name:{x.Name} Surname:{x.SurName} Age {x.Age}.")
                .OrderBy(x => x);

            Assert.Throws<InvalidOperationException>(() => human.ToList());
        }

        [Test]
        public void OrderBy___OrderByInterpolationStringAndReWrite__Correct()
        {
            var human = _context.Humans
                .Select(x => $" Name:{x.Name} Surname:{x.SurName} Age {x.Age}.")
                .ReWrite()
                .OrderBy(x => x);

            Assert.DoesNotThrow(() => human.ToList());
        }

        [Test]
        public void OrderBy___OrderByConcatString__Correct()
        {
            var human = _context.Humans
                .Select(x => " Name: " + x.Name + " Surname: " + x.SurName + " Age: " + x.Age + ".")
                .OrderBy(x => x);

            Assert.DoesNotThrow(() => human.ToList());
        }

        
    }
}