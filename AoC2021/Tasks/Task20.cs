using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCCommon;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AoC2021.Tasks
{
    public class Task20 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines();
            var algorithm = input[0].Select(x => x == '.' ? 0 : 1).ToArray();
            var img = new InfiniteImage(input.Skip(2).ToList());

            var proc = img.Process(algorithm);
            proc = proc.Process(algorithm);
            return GetResult(proc.Pixels.Values.Sum());
        }

        private void PrintImage(InfiniteImage img, int index)
        {
            var w = 220;
            var h = 220;
            var image = new Image<Rgba32>(w, h);
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    var px = x + img.XMin;
                    var py = y + img.YMin;
                    image[x,y] = img.GetPixel(px, py) == 0 ? Color.Black : Color.Yellow;
                }
            }
            image.SaveAsBmp(index + ".bmp");
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            var algorithm = input[0].Select(x => x == '.' ? 0 : 1).ToArray();
            var img = new InfiniteImage(input.Skip(2).ToList());
            for (var i = 0; i < 50; i++)
            {
                PrintImage(img, i);
                img = img.Process(algorithm);
            }
            return GetResult(img.Pixels.Values.Sum());
        }

        public class InfiniteImage
        {
            public InfiniteImage()
            {
            }

            public InfiniteImage(List<string> imageString)
            {
                for (var y = 0; y < imageString.Count; y++)
                {
                    for (var x = 0; x < imageString[y].Length; x++)
                    {
                        Pixels.Add((x,y), imageString[y][x] == '#' ? 1 : 0);
                    }
                }

                UpdateBounds();
            }

            private void UpdateBounds()
            {
                XMin = Pixels.Keys.Min(x => x.x) - 1;
                YMin = Pixels.Keys.Min(x => x.y) - 1;
                XMax = Pixels.Keys.Max(x => x.x) + 1;
                YMax = Pixels.Keys.Max(x => x.y) + 1;
            }

            public int Background { get; set; } = 0;

            public Dictionary<(int x, int y), int> Pixels { get; } = new();

            public int XMin {get; set; }
            public int YMin {get; set; }
            public int XMax {get; set; }
            public int YMax {get; set; }

            public int GetIndexFromCoords(int x, int y)
            {
                var index = string.Empty;
                for (var j = y - 1; j <= y + 1; j++)
                {
                    for (var i = x - 1; i <= x + 1; i++)
                    {
                        var p = GetPixel(i, j);
                        index += p.ToString();
                    }
                }
                return Convert.ToInt32(index, 2);
            }

            public int GetPixel(int x, int y)
            {
                return !Pixels.ContainsKey((x, y)) ? Background : Pixels[(x, y)];
            }

            public InfiniteImage Process(int[] algorithm)
            {
                var result = new InfiniteImage();
                for (var x = XMin; x <= XMax; x++)
                    for (var y = YMin; y <= YMax; y++)
                        result.Pixels.Add((x,y), algorithm[GetIndexFromCoords(x,y)]);
                result.Background = algorithm[0] == 1 ? 1-Background : 0;
                result.UpdateBounds();
                return result;       
            }
        }
    }
}
