using System;

namespace LeetCodeCN
{
    class Program
    {
        static void Main(string[] args)
        {
            //[3522,181,521,515,304,123,2512,312,922,407,146,1932,4037,2646,3871,269]
            // 5
            var result =
                new leetcode.Solution_698().CanPartitionKSubsets(new int[] { 3522, 181, 521, 515, 304, 123, 2512, 312, 922, 407, 146, 1932, 4037, 2646, 3871, 269 }, 5);
            Console.WriteLine(result);
        }
    }
}
