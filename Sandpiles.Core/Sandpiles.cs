﻿using System;

namespace Sandpiles.Core
{
    public class Sandpiles
    {
        public int Width { get; }
        public int Height { get; }
        public int[] DecayProbabilities { get; }
        public int[] GrowthProbabilities { get; }
        public int[,] Piles { get; protected set; }
        protected int[,] Diff;

        protected Random Random;

        public Sandpiles(int width, int height, int[] decayProbabilities, int[] growthProbabilities) {
           Width = width;
           Height = height;
           DecayProbabilities = decayProbabilities;
           GrowthProbabilities = growthProbabilities;
           Random = new Random((int)DateTime.Now.Ticks);
        }
        protected int GeneratePile(int row, int col) => Random.Next(0, 55);
        public void InitPiles(Func<int, int, int> generator = null) {
            Piles = new int[Width, Height];
            Diff = new int[Width, Height];
            generator = generator ?? GeneratePile;
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    Piles[i, j] = generator(i, j);
                }
            }
        }

        public void ComputeRound() {
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    var val = Piles[i, j];
                    if (Random.Next(0, DecayProbabilities[val]) == 0) {
                        Decay(i, j);
                    }
                    else if (Random.Next(0, GrowthProbabilities[val]) == 0) {
                        Grow(i, j);
                    }
                    Diff[i, j] = 0;
                }
            }
            for (var i = 0; i < Width; i++) {
                for (var j = 0; j < Height; j++) {
                    Piles[i, j] += Diff[i, j];
                    Diff[i, j] = 0;
                }
            }
        }

        private void Grow(int i, int j) {
            Diff[i, j] += 1;
        }
        private void Decay(int i, int j) {
            var inc = Piles[i, j] / 4;
            Diff[i, j] -= inc * 4;
            if (i > 0 && i < Width) {
                Diff[i - 1, j] += inc;
            }
            if (i < Width - 1) {
                Diff[i + 1, j] += inc;
            }
            if (j > 0 && j < Height) {
                Diff[i, j - 1] += inc;
            }
            if (j < Height - 1) {
                Diff[i, j + 1] += inc;
            }
        }
    }
}
