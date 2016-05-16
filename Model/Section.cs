﻿using System.Collections.Generic;

namespace Model
{
    public class Section : Procedure
    {
        public List<Procedure> Procedures { get; }

        public Section(CobolFile cobolFile, string name, int startIndex, int endIndex)
            : base(cobolFile, name, startIndex, endIndex)
        {
            Procedures = new List<Procedure>();
        }

        public override List<CobolTreeNode> GetNodes()
        {
            return new List<CobolTreeNode>(Procedures);
        }
    }
}
