using System;
using System.Collections.Generic;

namespace Lab08
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int[] nums = { 1, 2, 3, 4, 5, 9, 5, 3, 2, 5, 7, 1, 1, 3, 4, 6, 7, 8 };
            int target = 9;

            var results = ThreeSum(nums, target);

            if (results.Count > 0)
            {
                Console.WriteLine("Triplets found:");
                foreach (var triplet in results)
                {
                    Console.WriteLine($"[{string.Join(", ", triplet)}]");
                    //Console.Write($"[{string.Join(", ", triplet)}]");
                }
            }
            else
            {
                Console.WriteLine("No triplets found.");
            }
        }

        public static List<List<int>> ThreeSum(int[] nums, int target)
        {
            Array.Sort(nums);
            List<List<int>> results = new List<List<int>>();
            int n = nums.Length;

            for (int i = 0; i < n - 2; i++)
            {
                if (i > 0 && nums[i] == nums[i - 1])
                    continue; 

                int left = i + 1, right = n - 1;

                while (left < right)
                {
                    int sumVal = nums[i] + nums[left] + nums[right];

                    if (sumVal == target)
                    {
                        results.Add(new List<int> { nums[i], nums[left], nums[right] });

                        while (left < right && nums[left] == nums[left + 1]) left++;
                        while (left < right && nums[right] == nums[right - 1]) right--;

                        left++;
                        right--;
                    }
                    else if (sumVal < target)
                    {
                        left++;
                    }
                    else
                    {
                        right--;
                    }
                }
            }

            return results;
        }
    }
}