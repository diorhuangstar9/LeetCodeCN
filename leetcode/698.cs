using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LeetCodeCN.leetcode
{
    public class Solution_698
    {
        private HashSet<int> combinedIndex = new HashSet<int>();

        public bool CanPartitionKSubsets(int[] nums, int k)
        {
            // divided num is int
            var sum = 0;
            foreach (var num in nums)
                sum += num;
            int avg = sum / k;
            if (avg * k != sum)
                return false;
            //Console.WriteLine($"avg:{avg}");

            // build samesum subset dict
            var sameSumSubsetList = new List<string>();
            for (var i = 1; i <= nums.Length; i++)
            {
                var avgSumSubset = BuildSameSumSubsetDict(i, avg, 0, nums);
                if (avgSumSubset.Any())
                    sameSumSubsetList.AddRange(avgSumSubset);
            }
            if (sameSumSubsetList.Count < k)
                return false;
            // select k subsets in dict
            //return CanSubsetBeSelected(k, new List<int>(), nums.Length, 0, sameSumSubsetList);

            for (var i = 0; i <= sameSumSubsetList.Count - k; i++)
            {
                if (CanSubsetBeSelected(k - 1, sameSumSubsetList[i].Split(',').Select(x => int.Parse(x)).ToList(),
                    nums.Length, i, sameSumSubsetList))
                    return true;
            }

            //foreach (var subset in sameSumSubsetDict)
            //{
            //    Console.Write(subset.Key);
            //    Console.Write("(index):");
            //    Console.WriteLine(string.Join(';', subset.Value));

            //    Console.Write(subset.Key);
            //    Console.Write("(value):");
            //    Console.WriteLine(string.Join(';', subset.Value.Select(x =>
            //        string.Join(",", x.Split(",").Select(idx => nums[int.Parse(idx)])))));
            //}
            return false;
        }

        private bool CanSubsetBeSelected(int leftSumSubSetCnt, List<int> usedNumIndexes, int numsLength,
            int currentSubSetIndex, List<string> sameSumSubsetList)
        {
            if (leftSumSubSetCnt <= 0)
                return usedNumIndexes.Count >= numsLength;
            if (currentSubSetIndex >= sameSumSubsetList.Count)
                return false;
            if (leftSumSubSetCnt > sameSumSubsetList.Count - currentSubSetIndex)
                return false;

            var currentNotSelect = CanSubsetBeSelected(leftSumSubSetCnt, usedNumIndexes, numsLength,
                    currentSubSetIndex + 1, sameSumSubsetList);
            var currentNumIndexes = sameSumSubsetList[currentSubSetIndex].Split(',').
                Select(idx => int.Parse(idx)).ToList();
            var isIntersect = currentNumIndexes.Any(x => usedNumIndexes.Contains(x));
            if (isIntersect)
                return currentNotSelect;
            else if (currentNotSelect)
                return true;
            else
            {
                currentNumIndexes.AddRange(usedNumIndexes);
                var currentSelect = CanSubsetBeSelected(leftSumSubSetCnt - 1, currentNumIndexes, numsLength,
                    currentSubSetIndex + 1, sameSumSubsetList);
                return currentSelect;
            }

        }

        private IEnumerable<string> BuildSameSumSubsetDict(
            int sumNumCnt, int leftSum, int startIndex, int[] nums)
        {
            var sumSubSets = new List<string>();
            if (sumNumCnt <= 0)
                return sumSubSets;
            for (var i = startIndex; i < nums.Length; i++)
            {
                if (nums[i] > leftSum)
                    continue;
                else if (nums[i] == leftSum && sumNumCnt == 1)
                    sumSubSets.Add(i.ToString());
                else
                {
                    var nextSumSubSets = BuildSameSumSubsetDict(
                        sumNumCnt - 1, leftSum - nums[i], i + 1, nums);
                    if (nextSumSubSets.Any())
                        sumSubSets.AddRange(nextSumSubSets.Select(x => $"{i},{x}"));
                }
            }
            return sumSubSets;
        }

        private bool CanPartitionSubSet(IEnumerable<int> usedNumIndexes, int j)
        {
            return false;

        }

        public bool CanPartitionKSubsets_Deprecated(int[] nums, int k)
        {
            // divided num is int
            var sum = 0;
            foreach (var num in nums)
                sum += num;
            int avg = sum / k;
            if (avg * k != sum)
                return false;
            Console.WriteLine($"avg={avg}");
            // add all possible num to partitionSets
            List<int[]> allPartitionSets = null;
            foreach (var num in nums)
            {
                var newPartitionSets = new List<int[]>();
                if (allPartitionSets == null || allPartitionSets.Count <= 0)
                {
                    newPartitionSets.AddRange(HandlePartitionSets(null, k, num, avg));
                }
                else
                {
                    foreach (var originalPartitionSet in allPartitionSets)
                    {
                        newPartitionSets.AddRange(HandlePartitionSets(originalPartitionSet, k, num, avg));
                    }
                }
                foreach (var set in newPartitionSets)
                    Console.WriteLine(string.Join(',', set));
                allPartitionSets = newPartitionSets;
            }

            for (var i = allPartitionSets.Count - 1; i >= 0; i--)
            {
                var partitionSet = allPartitionSets[i];
                Console.WriteLine(string.Join(',', partitionSet));
                if (partitionSet.All(x => x == avg))
                    return true;
            }
            return false;
        }

        private List<int[]> HandlePartitionSets(int[] originalPartitionSet, int k, int num, int avg)
        {
            var newPartitionSets = new List<int[]>();
            for (var i = 0; i < k; i++)
            {
                var newPartitionSet = originalPartitionSet == null ?
                    new int[k] : (int[])originalPartitionSet.Clone();
                newPartitionSet[i] += num;
                if (newPartitionSet[i] <= avg)
                    newPartitionSets.Add(newPartitionSet);
            }

            return newPartitionSets;
        }

        public bool CanPartitionKSubsets2(int[] nums, int k)
        {
            // divided num is int
            var sum = 0;
            foreach (var num in nums)
                sum += num;
            int avg = sum / k;
            if (avg * k != sum)
                return false;

            var currentSum = 0;
            var currentSumStartIndex = 0;
            var currentIndex = 0;

            // check each num can be combined to divided num
            while (combinedIndex.Count < nums.Length)
            {
                if (combinedIndex.Contains(currentIndex))
                {
                    currentIndex++;
                    continue;
                }
                var num = nums[currentIndex];
                if (num > avg)
                    return false;
                if (num + currentSum < avg)
                {
                    currentSum += num;
                    combinedIndex.Add(currentIndex);
                    currentIndex++;
                }
                else if (num + currentSum == avg)
                {
                    currentSum = 0;
                    combinedIndex.Add(currentIndex);
                    currentIndex = currentSumStartIndex + 1;
                    currentSumStartIndex = currentIndex;
                }
                else
                {
                    currentIndex++;
                }
                if (currentIndex >= nums.Length)
                    return false;
            }
            return true;
        }
    }
}
