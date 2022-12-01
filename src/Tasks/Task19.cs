using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task19 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var ti = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";
            var scannerStrings = InitTaskString().Trim().Split("\n\n")
                .Select(x => x.Split("\n"))
                .Select(x => (Convert.ToInt32(x[0].Split(" ")[2]), x.Skip(1).ToList())).ToList();
                
            var scanners = scannerStrings
                .AsParallel()
                .Select(x => (x.Item1,
                    x.Item2.Select(c => c.Split(',')
                            .Select(p => Convert.ToInt32(p)).ToArray())
                        .Select(c => new Beacon(x.Item1, c[0], c[1], c[2])).ToArray()))
                .Select(x => new Scanner(x.Item1, x.Item2))
                .ToList();

            var beacons = new HashSet<Beacon>();
            foreach (var b in scanners.SelectMany(x => x.Beacons))
                beacons.Add(b);
            var bc = 0;

            for (var s = 0; s < scanners.Count-1; s++)
            {
                for (var o = s + 1; o < scanners.Count; o++)
                {
                    Console.WriteLine(s + " " + o);
                    var scanner = scanners[s];
                    var other = scanners[o];
                    foreach (var b in DoMatch(scanner, other))
                    {
                        var ra = beacons.Remove(b.Item1); 
                        var rb = beacons.Remove(b.Item2);
                        if (ra && rb)
                            bc++;
                    }
                }
            }

            return GetResult(bc + beacons.Count);
        }

        private List<(Beacon, Beacon)> DoMatch(Scanner scanner, Scanner other)
        {
            var resList = new List<(Beacon, Beacon)>();

            for (var i = 0; i < 24; i++)
            {
                var matches = new List<((int x, int y, int z, Beacon b) a, (int x, int y, int z, Beacon b) match)>();
                foreach (var n in scanner.Beacons)
                {
                    foreach (var on in other.Beacons)
                    {
                        var onV = on.NeighborVectors[i];
                        foreach (var a in n.NeighborVectors[0])
                        {
                            var match = onV.Where(p => p.x == a.x && p.y == a.y && p.z == a.z).ToList();
                            if (match.Any()) 
                                matches.Add((a, match.First()));
                        }
                        
                        if (matches.Count >= 12)
                        {
                            foreach (var (a, b) in matches)
                            {
                                resList.Add((a.b, b.b));
                            }
                            return resList;
                        }
                    }
                }
            }

            return resList;
        }

        public override TaskResult RunPartTwo()
        {
            var ti = @"--- scanner 0 ---
404,-588,-901
528,-643,409
-838,591,734
390,-675,-793
-537,-823,-458
-485,-357,347
-345,-311,381
-661,-816,-575
-876,649,763
-618,-824,-621
553,345,-567
474,580,667
-447,-329,318
-584,868,-557
544,-627,-890
564,392,-477
455,729,728
-892,524,684
-689,845,-530
423,-701,434
7,-33,-71
630,319,-379
443,580,662
-789,900,-551
459,-707,401

--- scanner 1 ---
686,422,578
605,423,415
515,917,-361
-336,658,858
95,138,22
-476,619,847
-340,-569,-846
567,-361,727
-460,603,-452
669,-402,600
729,430,532
-500,-761,534
-322,571,750
-466,-666,-811
-429,-592,574
-355,545,-477
703,-491,-529
-328,-685,520
413,935,-424
-391,539,-444
586,-435,557
-364,-763,-893
807,-499,-711
755,-354,-619
553,889,-390

--- scanner 2 ---
649,640,665
682,-795,504
-784,533,-524
-644,584,-595
-588,-843,648
-30,6,44
-674,560,763
500,723,-460
609,671,-379
-555,-800,653
-675,-892,-343
697,-426,-610
578,704,681
493,664,-388
-671,-858,530
-667,343,800
571,-461,-707
-138,-166,112
-889,563,-600
646,-828,498
640,759,510
-630,509,768
-681,-892,-333
673,-379,-804
-742,-814,-386
577,-820,562

--- scanner 3 ---
-589,542,597
605,-692,669
-500,565,-823
-660,373,557
-458,-679,-417
-488,449,543
-626,468,-788
338,-750,-386
528,-832,-391
562,-778,733
-938,-730,414
543,643,-506
-524,371,-870
407,773,750
-104,29,83
378,-903,-323
-778,-728,485
426,699,580
-438,-605,-362
-469,-447,-387
509,732,623
647,635,-688
-868,-804,481
614,-800,639
595,780,-596

--- scanner 4 ---
727,592,562
-293,-554,779
441,611,-461
-714,465,-776
-743,427,-804
-660,-479,-426
832,-632,460
927,-485,-438
408,393,-506
466,436,-512
110,16,151
-258,-428,682
-393,719,612
-211,-452,876
808,-476,-593
-575,615,604
-485,667,467
-680,325,-822
-627,-443,-432
872,-547,-609
833,512,582
807,604,487
839,-516,451
891,-625,532
-652,-548,-490
30,-46,-14";
            var scannerStrings = ti.Trim().Split("\n\n")
                .Select(x => x.Split("\n"))
                .Select(x => (Convert.ToInt32(x[0].Split(" ")[2]), x.Skip(1).ToList())).ToList();
                
            var scanners = scannerStrings
                .AsParallel()
                .Select(x => (x.Item1,
                    x.Item2.Select(c => c.Split(',')
                            .Select(p => Convert.ToInt32(p)).ToArray())
                        .Select(c => new Beacon(x.Item1, c[0], c[1], c[2])).ToArray()))
                .Select(x => new Scanner(x.Item1, x.Item2))
                .ToList();

            var beacons = new HashSet<Beacon>();
            foreach (var b in scanners.SelectMany(x => x.Beacons))
                beacons.Add(b);
            var bc = 0;

            for (var s = 0; s < scanners.Count-1; s++)
            {
                for (var o = s + 1; o < scanners.Count; o++)
                {
                    Console.WriteLine(s + " " + o);
                    var scanner = scanners[s];
                    var other = scanners[o];
                    foreach (var b in DoMatch(scanner, other))
                    {
                        var ra = beacons.Remove(b.Item1); 
                        var rb = beacons.Remove(b.Item2);
                        if (ra && rb)
                            bc++;
                    }
                }
            }

            return GetResult(bc + beacons.Count);
        }
    }

    public class Beacon
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int ScannerId { get; set; }

        public (int x, int y, int z) Coords => (X, Y, Z);

        public List<(int x, int y, int z)> Permutations => Beaconize(this).ToList();

        public List<Beacon> Neighbors { get; } = new List<Beacon>();

        public List<HashSet<(int x, int y, int z, Beacon b)>> NeighborVectors { get; } = Enumerable.Repeat(0, 24).Select(x => new HashSet<(int, int, int, Beacon)>()).ToList();

        private IEnumerable<(int x, int y, int z)> Beaconize(Beacon beacon)
        {
            return BeaconizeWithDups(beacon).Distinct();
        }

        private IEnumerable<(int x, int y, int z)> BeaconizeWithDups(Beacon beacon)
        {
            var rx = new Func<(int x, int y, int z), (int x, int y, int z)>(i => (i.x, i.z, -i.y));
            var ry = new Func<(int x, int y, int z), (int x, int y, int z)>(i => (-i.z, i.y, i.x));
            var rz = new Func<(int x, int y, int z), (int x, int y, int z)>(i => (-i.y, i.x, i.z));

            var rots = new[] { rx, ry, rz };
            
            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            for (var k = 0; k < 4; k++)
            {
                yield return Enumerable.Repeat(rots[0], i).Concat(Enumerable.Repeat(rots[1], j))
                    .Concat(Enumerable.Repeat(rots[2], k)).Aggregate(beacon.Coords, (current, rot) => rot(current));
            }
        }
        
        public Beacon(int scannerId, int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            ScannerId = scannerId;
        }

        protected bool Equals(Beacon other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && ScannerId == other.ScannerId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Beacon)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, ScannerId);
        }

        public void SetNeighbor(Beacon other)
        {
            Neighbors.Add(other);
            for (var i = 0; i < 24; i++)
            {
                var (x, y, z) = Diff(other.Permutations[i], Permutations[i]);
                NeighborVectors[i].Add((x, y, z, other));
            }
        }

        private (int x, int y, int z) Diff((int x, int y, int z) a, (int x, int y, int z) b)
        {
            return (a.x - b.x, a.y - b.y, a.z - b.z);
        }
    }

    public class Scanner
    {
        public int Name { get; set; }
        public List<Beacon> Beacons { get; } = new();
        
        public Scanner(int name, IEnumerable<Beacon> values)
        {
            Name = name;
            Beacons.AddRange(values);
            foreach (var beacon in Beacons)
            {
                foreach (var other in Beacons.Where(x => !Equals(x, beacon)))
                {
                    beacon.SetNeighbor(other);
                }
            }

            Console.WriteLine($"Scanner {name} ready");
        }
    }
}