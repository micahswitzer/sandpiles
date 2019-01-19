using System;

namespace Sandpiles.Cli
{
    class Program
    {
        static void Main()
        {
            var valRange = 55;
            int[] probDecay = new int[valRange];
            int[] probGrowth = new int[valRange];

            for (var i = 4; i < valRange; i++) {
                var prob = (50 + (i - 4)) * 200_000;
                probDecay[i] = prob;
            }
            for (var i = 3; i < valRange - 1; i++) {
                var prob = (int)(Math.Pow(0.762718, i) * 2253927.46861);
                probGrowth[i] = prob;
            }

            var piles = new Core.Sandpiles(100, 100, probDecay, probGrowth);
            piles.Init();

            while (true) {
                piles.ComputeRound();
            }
        }
    }
}
