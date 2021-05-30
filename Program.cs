using System;

namespace LeetCodeCN
{
    class Program
    {
        static void Main(string[] args)
        {
            var result =
                new leetcode.Solution_698().CanPartitionKSubsets(new int[] { 5, 8, 3, 8, 1, 6, 1, 5, 1, 6 }, 4);
            Console.WriteLine(result);
        }
    }
}
