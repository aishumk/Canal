﻿namespace Model.File
{
    /// <summary>
    /// Abstract class for Identification/Environment/Data/Procedure Division
    /// </summary>
    public abstract class Division : CobolTreeNode
    {
        public override int StartIndex { get; protected set; }

        public override int EndIndex { get; protected set; }

        protected Division(CobolFile cobolFile, string nodeName, int beginIndex, int endIndex)
            : base(cobolFile, nodeName)
        {
            StartIndex = beginIndex;
            EndIndex = endIndex;
        }
    }
}