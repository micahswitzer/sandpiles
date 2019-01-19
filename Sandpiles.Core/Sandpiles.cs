using System;

namespace Sandpiles.Core
{
    public class Sandpiles
    {
        public int Width { get; }
        public int Height { get; }
        public int[] DecayProbabilities { get; }
        public int[] GrowthProbabilities { get; }
        public int[][] Piles { get; }

        public Sandpiles(int width, int height, int[] decayProbabilities, int[] growthProbabilities) {
           Width = width;
           Height = height;
           DecayProbabilities = decayProbabilities;
           GrowthProbabilities = growthProbabilities;
        }

        public void InitPiles() {
            
        }
    }
}
