﻿// <copyright file="ResizeTests.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Tests
{
    using System.IO;

    using Xunit;

    public class ResizeTests : FileTestBase
    {
        private const string path = "TestOutput/Resize";

        public static readonly TheoryData<string, IResampler> ReSamplers =
            new TheoryData<string, IResampler>
            {
                { "Bicubic", new BicubicResampler() },
                { "Triangle", new TriangleResampler() },
                { "NearestNeighbor", new NearestNeighborResampler() },
                // Perf: Enable for local testing only
                //{ "Box", new BoxResampler() },
                //{ "Lanczos3", new Lanczos3Resampler() },
                //{ "Lanczos5", new Lanczos5Resampler() },
                //{ "Lanczos8", new Lanczos8Resampler() },
                //{ "MitchellNetravali", new MitchellNetravaliResampler() },
                //{ "Hermite", new HermiteResampler() },
                //{ "Spline", new SplineResampler() },
                //{ "Robidoux", new RobidouxResampler() },
                //{ "RobidouxSharp", new RobidouxSharpResampler() },
                //{ "RobidouxSoft", new RobidouxSoftResampler() },
                //{ "Welch", new WelchResampler() }
            };

        [Theory]
        [MemberData("ReSamplers")]
        public void ImageShouldResize(string name, IResampler sampler)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        image.Resize(image.Width / 2, image.Height / 2, sampler, false, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWidthAndKeepAspect()
        {
            const string name = "FixedWidth";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        image.Resize(image.Width / 3, 0, new TriangleResampler(), false, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeHeightAndKeepAspect()
        {
            const string name = "FixedHeight";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        image.Resize(0, image.Height / 3, new TriangleResampler(), false, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithCropMode()
        {
            const string name = "Crop";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(image.Width / 2, image.Height)
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithPadMode()
        {
            const string name = "Pad";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(image.Width + 200, image.Height),
                            Mode = ResizeMode.Pad
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithBoxPadMode()
        {
            const string name = "BoxPad";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(image.Width + 200, image.Height + 200),
                            Mode = ResizeMode.BoxPad
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithMaxMode()
        {
            const string name = "Max";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(300, 300),
                            Mode = ResizeMode.Max
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithMinMode()
        {
            const string name = "Min";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(image.Width - 50, image.Height - 25),
                            Mode = ResizeMode.Min
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Fact]
        public void ImageShouldResizeWithStretchMode()
        {
            const string name = "Stretch";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (string file in Files)
            {
                using (FileStream stream = File.OpenRead(file))
                {
                    string filename = Path.GetFileNameWithoutExtension(file) + "-" + name + Path.GetExtension(file);

                    Image image = new Image(stream);
                    using (FileStream output = File.OpenWrite($"{path}/{filename}"))
                    {
                        ResizeOptions options = new ResizeOptions()
                        {
                            Size = new Size(image.Width - 200, image.Height),
                            Mode = ResizeMode.Stretch
                        };

                        image.Resize(options, this.ProgressUpdate)
                             .Save(output);
                    }
                }
            }
        }

        [Theory]
        [InlineData(-2, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(2, 0)]
        [InlineData(2, 0)]
        public static void Lanczos3WindowOscillatesCorrectly(float x, float expected)
        {
            Lanczos3Resampler sampler = new Lanczos3Resampler();
            float result = sampler.GetValue(x);

            Assert.Equal(result, expected);
        }
    }
}