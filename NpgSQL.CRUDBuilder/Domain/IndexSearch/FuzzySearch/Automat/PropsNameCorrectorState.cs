using System;

namespace NpgSQL.CRUDBuilder.Domain.IndexSearch.FuzzySearch.Automat
{
    internal readonly struct PropsNameCorrectorState
    {
        public readonly TrieNode Node;
        
        public readonly int? AutomataState;
        
        public readonly int AutomataOffset;

        public PropsNameCorrectorState(TrieNode node, int state, int offset)
        {
            Node = node;
            AutomataState = state;
            AutomataOffset = offset;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PropsNameCorrectorState searchState))
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