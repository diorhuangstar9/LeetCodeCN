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
            // add all possible num to partitionSets
            List<int[]> allPartitionSets = null;
            foreach (var num in nums)
            {
                var newPartitionSets = new List<int[]>();
                if (allPartitionSets == null || allPartitionSets.Count <= 0)
                {
                    newPartitionSets.AddRange(HandlePartitionSets(null, k, num));
                }
                else
                {
                    foreach (var originalPartitionSet in allPartitionSets)
                    {
                        newPartitionSets.AddRange(HandlePartitionSets(originalPartitionSet, k, num));
                    }
                }
                allPartitionSets = newPartitionSets;
            }

            for (var i = allPartitionSets.Count - 1; i >= 0; i--)
            {
                var partitionSet = allPartitionSets[i];
                //Console.WriteLine(string.Join(',', partitionSet));
                if (partitionSet.All(x => x == avg))
                    return true;
            }
            return false;
        }

        private List<int[]> HandlePartitionSets(int[] originalPartitionSet, int k, int num)
        {
            var newPartitionSets = new List<int[]>();
            for (var i = 0; i < k; i++)
            {
                var newPartitionSet = originalPartitionSet == null ?
                    new int[k] : (int[])originalPartitionSet.Clone();
                newPartitionSet[i] += num;
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
