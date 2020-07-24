using System;
using NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms.FuzzySearch.TrieDataStructure;

namespace NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms.FuzzySearch.Automat
{
    internal readonly struct FuzzyCorrectorState
    {
        internal readonly TrieNode Node;
        
        internal readonly int? AutomataState;
        
        internal readonly int AutomataOffset;

        internal FuzzyCorrectorState(TrieNode node, int state, int offset)
        {
            Node = node;
            AutomataState = state;
            AutomataOffset = offset;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FuzzyCorrectorState searchState))
                return false;
            
            return searchState.AutomataOffset == AutomataOffset
                   && searchState.AutomataState == AutomataState
                   && searchState.Node == Node;
        }

        public override int GetHashCode()
        {
            var hash = AutomataState ^ AutomataOffset ^ Node.GetHashCode();
            
            if(hash is null)
                throw new NullReferenceException();

            return hash.Value;
        }
    }
}