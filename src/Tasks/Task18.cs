using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task18 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines().Select(SnailPair.ToSnailExpression).ToList();
            return GetResult(input.Aggregate((x, y) => x + y).Magnitude());
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines().Select(SnailPair.ToSnailExpression).ToList();
            return GetResult(input.Select(item =>
                input.Where(x => x != item).Select(otherItem => (item + otherItem).Magnitude()).Max()).Max());
        }
    }

    public interface SnailExpression
    {
        public SnailPair Parent { get; set; }
        public int Magnitude();
    } 
    
    public class SnailPair : SnailExpression 
    {
        public SnailExpression Left { get; set; }
        public SnailExpression Right { get; set; }
        public SnailPair Parent { get; set; }

        public int ParentCount => GetParents().Count();

        public static SnailPair operator +(SnailPair left, SnailPair right)
        {
            var res = new SnailPair(ToSnailExpression(left.ToString()), ToSnailExpression(right.ToString()), null);

            while (res.Reduce())
            {
            }
            
            return res;
        }

        private bool Reduce()
        {
            var flattened = this.Traverse().ToList();
            if (flattened.FirstOrDefault(x => x is SnailPair pair && pair.CanExplode) is SnailPair exploder)
            {
                exploder.Explode();
                return true;
            }
            else if (flattened.FirstOrDefault(x => x is SnailNumber num && num.CanSplit) is SnailNumber number)
            {
                number.Split();
                return true;
            }

            return false;
        }
        
        public static SnailPair ToSnailExpression(string input)
        {
            var stack = new Stack<SnailPair>();
            SnailPair current = null;
            SnailPair parent = null;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '[')
                {
                    parent = current;
                    if (current != null)
                        stack.Push(current);
                    current = new SnailPair(parent);
                    if (parent != null)
                    {
                        if (parent?.Left == null)
                            parent.Left = current;
                        else if (parent?.Right == null)
                            parent.Right = current;
                    }
                } 
                else if (c == ']')
                {
                    if (!stack.Any())
                        return current;
                    current = stack.Pop();
                    parent = current.Parent;
                } 
                else if (c is >= '0' and <= '9')
                {
                    if (current.Left == null)
                        current.Left = new SnailNumber(c - '0', current);
                    else
                        current.Right = new SnailNumber(c - '0', current);
                }
            }

            return null;
        }

        public IEnumerable<SnailExpression> Traverse()
        {
            var items = new[] { Left, Right };
            foreach (var item in items)
            {
                switch (item)
                {
                    case SnailNumber num:
                        yield return num;
                        break;
                    case SnailPair pair:
                    {
                        yield return pair;
                        foreach (var subItem in pair.Traverse())
                            yield return subItem;
                        break;
                    }
                }
            }
        }

        private IEnumerable<(SnailPair pair, SnailPairSide side)> GetParents()
        {
            if (Parent == null)
                return new List<(SnailPair pair, SnailPairSide side)>();
            return Parent.GetParents().Prepend(Parent.Left == this ? (Parent, SnailPairSide.Left) : (Parent, SnailPairSide.Right));
        }

        private SnailNumber GetSibling(SnailPairSide side)
        {
            if (Parent == null)
                return null;
            if (Parent.Left == this && side == SnailPairSide.Right || Parent.Right == this && side == SnailPairSide.Left)
            {
                var exp = side == SnailPairSide.Left ? Parent.Left : Parent.Right;
                if (exp is SnailNumber num)
                    return num;
                
                return ((SnailPair)exp).GetMostSidedChild((SnailPairSide)(SnailPairSide.Right - side));
            }
            return Parent.GetSibling(side);
        }

        private SnailNumber GetMostSidedChild(SnailPairSide side)
        {
            var exp = side == SnailPairSide.Left ? Left : Right;
            if (exp is SnailNumber num)
                return num;
            return ((SnailPair)exp).GetMostSidedChild(side);
        }

        public SnailPair(SnailExpression left, SnailExpression right, SnailPair parent)
        {
            Left = left;
            Right = right;
            Parent = parent;
            Left.Parent = this;
            Right.Parent = this;
        }

        public SnailPair(SnailPair parent)
        {
            Parent = parent;
        }

        public bool CanExplode => Left is SnailNumber && Right is SnailNumber && ParentCount >= 4;
        
        public void Explode()
        {
            if (Left is SnailNumber lnum && Right is SnailNumber rnum)
            {
                var left = GetSibling(SnailPairSide.Left);
                var right = GetSibling(SnailPairSide.Right);
                if (left != null)
                    left.Number += lnum.Number;
                if (right != null)
                    right.Number += rnum.Number;
            }

            if (Parent.Left == this)
                Parent.Left = new SnailNumber(0, Parent);
            else
                Parent.Right = new SnailNumber(0, Parent);
        }
        
        public int Magnitude()
        {
            return Left.Magnitude() * 3 + Right.Magnitude() * 2;
        }

        public override string ToString()
        {
            return $"[{Left},{Right}]";
        }
    }

    internal enum SnailPairSide
    {
        Left = 0,
        Right = 1
    }

    public class SnailNumber : SnailExpression
    {
        public SnailNumber(int number, SnailPair parent)
        {
            Number = number;
            Parent = parent;
        }

        public SnailPair Parent { get; set; }
        
        public int Number { get; set; }

        public bool CanSplit => Number > 9;

        public void Split()
        {
            var l = Number / 2;
            var r = (Number + 1) / 2;
            if (Parent.Left == this)
                Parent.Left = new SnailPair(new SnailNumber(l, null), new SnailNumber(r, null), Parent);
            else
                Parent.Right = new SnailPair(new SnailNumber(l, null), new SnailNumber(r, null), Parent);
        }

        public int Magnitude()
        {
            return Number;
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}