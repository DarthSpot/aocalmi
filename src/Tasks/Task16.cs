using System;
using System.Collections.Generic;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task16 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskString().Trim().Select(x => Convert.ToString(Convert.ToInt32(x.ToString(), 16), 2).PadLeft(4, '0'))
                .Aggregate((x, y) => x + y);

            var reader = new HexCodeReader(input);
            var packageTree = new List<Package>();
            var stack = new Stack<Operator>();

            while (reader.CanRead)
            {
                var (version, type) = reader.ReadHeader();
                Package pack = null;
                if (type == 4)
                {
                    var litVal = new LiteralValue(type, version);
                    pack = litVal;
                    litVal.Value = reader.ReadValue();
                }
                else
                {
                    var opPack = new Operator(type, version);
                    pack = opPack;
                    if (reader.ReadInt(1) == 0)
                    {
                        var length = reader.ReadInt(15);
                    }
                    else
                    {
                        var length = reader.ReadInt(11);
                    }
                }
                if (stack.TryPeek(out var op))
                    op.SubPackages.Add(pack);
                packageTree.Add(pack);
                    
            }
            
            return GetResult(packageTree.Sum(x => x.Version));
        }

        internal class HexCodeReader
        {
            public string Input { get; }

            private int Position = 0;
            public bool CanRead => Position < Input.Length && RemainingString.Any(x => x != '0');
            private string RemainingString => Input.Substring(Position);
            public HexCodeReader(string input)
            {
                Input = input;
            }

            public string Read(int bits)
            {
                if (RemainingString.Length >= bits)
                {
                    var res = Input.Substring(Position, bits);
                    Position += bits;
                    return res;
                }
                else
                {
                    return string.Empty;
                }
            }
            
            public int ReadInt(int bits)
            {
                return Convert.ToInt32(Read(bits), 2);
            }
            
            public (int version, int packageId) ReadHeader()
            {
                if (RemainingString.Length < 6)
                    throw new Exception();
                var ver = ReadInt(3);
                var type = ReadInt(3);
                return (ver, type);
            }

            public long ReadValue()
            {
                var flag = 1;
                var nums = new List<string>();
                while (flag != 0)
                {
                    flag = ReadInt(1);
                    nums.Add(Read(4));
                }
                return Convert.ToInt64(nums.Aggregate((x, y) => x + y), 2);
            }

            public IEnumerable<Package> ReadPackages(int maxLength = 0)
            {
                var readBytes = 0;
                while (CanRead && (maxLength == 0 || readBytes < maxLength))
                { 
                    var p = Position;
                    var (version, type) = ReadHeader();
                    if (type == 4)
                    {
                        var litVal = new LiteralValue(type, version);
                        litVal.Value = ReadValue();
                        var bits = Position - p;
                        litVal.PackageLength = bits; 
                        readBytes += bits;
                        yield return litVal;
                    }
                    else
                    {
                        var opPack = new Operator(type, version);
                        if (ReadInt(1) == 0)
                        {
                            var length = ReadInt(15);
                            opPack.SubPackages.AddRange(ReadPackages(length));
                        }
                        else
                        {
                            var count = ReadInt(11);
                            opPack.SubPackages.AddRange(ReadPackages().Take(count));
                        }

                        var bits = Position - p;
                        opPack.PackageLength = bits;
                        readBytes += bits;
                        yield return opPack;
                    }
                }
            }
        }

        internal abstract class Package
        {
            public int TypeId { get; }
            public int Version { get; }
            
            public abstract int PackageLength { get; set; }

            protected Package(int typeId, int version)
            {
                TypeId = typeId;
                Version = version;
            }

            public abstract long Evaluate();
        }

        private class Operator : Package
        {
            public List<Package> SubPackages { get; } = new();

            public Operator(int typeId, int version) : base(typeId, version)
            {
            }
            
            public override int PackageLength { get; set; }

            public override long Evaluate()
            {
                return TypeId switch
                {
                    0 => SubPackages.Select(x => x.Evaluate()).Sum(),
                    1 => SubPackages.Select(x => x.Evaluate()).Aggregate((x, y) => x * y),
                    2 => SubPackages.Min(x => x.Evaluate()),
                    3 => SubPackages.Max(x => x.Evaluate()),
                    5 => SubPackages[0].Evaluate() > SubPackages[1].Evaluate() ? 1 : 0,
                    6 => SubPackages[0].Evaluate() < SubPackages[1].Evaluate() ? 1 : 0,
                    7 => SubPackages[0].Evaluate() == SubPackages[1].Evaluate() ? 1 : 0
                };
            }
        }
        
        private class LiteralValue : Package
        {
            public long Value { get; set; }
            public LiteralValue(int typeId, int version) : base(typeId, version)
            {
            }

            public override int PackageLength { get; set; }

            public override long Evaluate()
            {
                return Value;
            }
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskString().Trim().Select(x => Convert.ToString(Convert.ToInt32(x.ToString(), 16), 2).PadLeft(4, '0'))
                .Aggregate((x, y) => x + y);

            var reader = new HexCodeReader(input);

            var packages = reader.ReadPackages().Single();
            
            return GetResult(packages.Evaluate());
        }
    }
}