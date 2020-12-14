using System;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt").Select(x => long.Parse(x)).ToArray();
            Console.WriteLine(Part1(lines));
            Console.WriteLine(Part2(lines));
        }

        static bool IsValid(int currentIndex, long[] numbers, int preambleLength)
        {
            long n = numbers[currentIndex];
            for(int i = currentIndex - preambleLength; i < currentIndex; i++)
                for(int j = i+1; j < currentIndex; j++)
                    if(numbers[i] + numbers[j] == n)
                        return true;
            return false;
        }

        static long Part1(long[] nums)
        {
            int preambleLength = 25;
            for(int i = 25; i<nums.Length; i++)
            {
                if(!IsValid(i, nums, preambleLength))
                    return nums[i];
            }
            return -1;
        }

        static long Part2(long[] nums)
        {
            int preambleLength = 25;
            long num = -1;
            int ind = -1;
            for(int i = 25; i<nums.Length; i++)
            {
                if(!IsValid(i, nums, preambleLength))
                {
                    num = nums[i];
                    ind = i;
                    break;
                }
            }
            if(num < 0) return -1;
            long[] sumPref = new long[ind+1];
            sumPref[0] = 0;
            for(int l = 0; l < ind; l++)
                sumPref[l+1] = sumPref[l] + nums[l]; //sum[i+1] = nums[0]+nums[1]+nums[2]+...+nums[i]
            for(int l = 2; l < ind - 2; l++)
            {
                for(int s = 0; s < ind - l; s++)
                {
                    long sum = sumPref[s+l] - sumPref[s]; //nums[s+l-1] + nums[s+l-2] + ... + nums[s]
                    if(sum == num)
                    {
                        long min = nums[s], max = nums[s];
                        for(int k = s+1; k < s + l; k++)
                        {
                            if(nums[k] > max) max = nums[k];
                            if(nums[k] < min) min = nums[k];
                        }
                        return min + max;
                    }
                }
            }
            return -1;
        }
    }
}
