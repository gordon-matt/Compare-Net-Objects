﻿using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

#nullable enable

namespace KellermanSoftware.CompareNetObjectsTests.TestClasses
{
    internal sealed class GenericCollectionWithLists
    {
        public List<List<int>?> ListOfLists { get; }

        public GenericCollectionWithLists(List<List<int>?> listOfLists)
        {
            ListOfLists = listOfLists;
        }

        public static GenericCollectionWithLists Create()
        {
            var listOfLists = PrepareList();
            return new GenericCollectionWithLists(listOfLists);
        }

        private static List<List<int>?> PrepareList()
        {
            var listOfLists = new List<List<int>?>(2);
            for (int i = 0; i < listOfLists.Capacity; ++i)
            {
                listOfLists.Add(i % 3 == 0 ? null : new List<int>(Enumerable.Range(0, i + 1)));
            }

            return listOfLists;
        }

        public void ManualCompare(GenericCollectionWithLists? other)
        {
            Assert.NotNull(other);
            if (other is null) // To suppress null warning.
            {
                Assert.Fail("Invalid object to compare.");
                return;
            }

            Assert.AreEqual(ListOfLists.Count, other.ListOfLists.Count);
            for (int i = 0; i < ListOfLists.Count; i++)
            {
                List<int>? ch1 = ListOfLists[i];
                List<int>? ch2 = other.ListOfLists[i];
                CollectionAssert.AreEqual(ch1, ch2);
            }
        }

        public void CompareObjects(GenericCollectionWithLists? other)
        {
            CompareLogic compareLogic = new()
            {
                Config =
                {
                    ComparePrivateProperties = false,
                    IgnoreObjectTypes = false,
                    IgnoreCollectionOrder = true // Should be true
                }
            };
            var result = compareLogic.Compare(this, other);
            if (!result.AreEqual)
            {
                Assert.Fail(result.DifferencesString);
            }
        }
    }
}