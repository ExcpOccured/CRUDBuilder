using System.Collections.Generic;
using System.Linq;
using NpgSQL.CRUDBuilder.Domain.IndexSearch.FuzzySearch.Automat;
using NpgSQL.CRUDBuilder.Domain.IndexSearch.FuzzySearch.TrieDataStructure;

namespace NpgSQL.CRUDBuilder.Domain.IndexSearch.FuzzySearch
{
    /// <summary>
    /// Fuzzy indexer uses the Levenshtein automaton and FB-Trie algorithm
    /// to find possible corrections for the given garbled characters
    /// </summary>
    internal class FuzzyIndexer
    {
        Trie ForwardDictionary = new Trie();

        Trie BackwardDictionary = new Trie();

        public FuzzyIndexer() { }

        public FuzzyIndexer(IEnumerable<string> charactersArray)
        {
            foreach (var characters in charactersArray)
            {
                if (string.IsNullOrEmpty(characters)) continue;

                ForwardDictionary.InsertCharacters(characters);
                BackwardDictionary.InsertCharacters(new string(characters.Reverse().ToArray()));
            }
        }

        public void InsertCharacters(string characters)
        {
            ForwardDictionary.InsertCharacters(characters);
            BackwardDictionary.InsertCharacters(new string(characters.Reverse().ToArray()));
        }

        public IList<string> GetCorrections2T(string typo)
        {
            var corrections = new List<string>();

            if (string.IsNullOrEmpty(typo))
            {
                return corrections;
            }

            if (typo.Length <= Constants.MaxLevTLength)
            {
                return GetCorrectionStrings(typo, ForwardDictionary.root, 2).ToList();
            }

            string left;
            string right;
            string rleft;
            string rright;

            var llen = PrepareSubstrings(typo, out left, out right, out rleft, out rright);

            var lnode = ForwardDictionary.GetNode(left);
            if (lnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(right, lnode, Constants.MaxLevTLength));
            }

            var rnode = BackwardDictionary.GetNode(rright);
            if (rnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(rleft, rnode, Constants.MaxLevTLength)
                    .Select(letter => new string(letter.Reverse().ToArray())));
            }

            foreach (var node in GetCorrectionNodes(left, ForwardDictionary.root, 1, false))
            {
                corrections.AddRange(GetCorrectionStrings(right, node, 1));
            }

            var buffer = typo.ToCharArray();
            var letter = buffer[llen - 1];
            buffer[llen - 1] = buffer[llen];
            buffer[llen] = letter;

            var transposedTypo = new string(buffer);

            PrepareSubstrings(transposedTypo, out left, out right, out rleft, out rright);

            lnode = ForwardDictionary.GetNode(left);
            if (lnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(right, lnode, 1));
            }

            rnode = BackwardDictionary.GetNode(rright);
            if (rnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(rleft, rnode, 1)
                    .Select(correction => new string(correction.Reverse().ToArray())));
            }

            return corrections.Distinct().ToList();
        }

        public IList<string> GetCorrections1T(string typo)
        {
            if (typo.Length <= 2)
            {
                return GetCorrectionStrings(typo, ForwardDictionary.root, 1).ToList();
            }

            var corrections = new List<string>();

            string left;
            string right;
            string rleft;
            string rright;

            var llen = PrepareSubstrings(typo, out left, out right, out rleft, out rright);

            var lnode = ForwardDictionary.GetNode(left);
            if (lnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(right, lnode, 1));
            }

            var rnode = BackwardDictionary.GetNode(rright);
            if (rnode != null)
            {
                corrections.AddRange(GetCorrectionStrings(rleft, rnode, 1)
                    .Select(x => new string(x.Reverse().ToArray())));
            }

            var buffer = typo.ToCharArray();
            var letter = buffer[llen - 1];
            buffer[llen - 1] = buffer[llen];
            buffer[llen] = letter;

            var trieNode = ForwardDictionary.GetChars(new string(buffer));
            if (trieNode != null)
            {
                corrections.Add(trieNode.Key);
            }

            return corrections.Distinct().ToList();
        }

        private IEnumerable<string> GetCorrectionStrings(string typo, TrieNode start, int editDistance)
        {
            return GetCorrectionNodes(typo, start, editDistance).Select(trieNode => trieNode.Key);
        }

        private IEnumerable<TrieNode> GetCorrectionNodes(string typo, TrieNode start, int editDistance,
            bool includeOnlyCharacters = true)
        {
            var corrections = new List<TrieNode>();

            if (string.IsNullOrEmpty(typo))
            {
                return corrections;
            }

            var automata = new LevTAutomatImitation(typo, editDistance);
            var stack = new Stack<Automat.FuzzyCorrectorState>();

            stack.Push(new Automat.FuzzyCorrectorState(start, 0, 0));

            while (stack.Count > 0)
            {
                var state = stack.Pop();

                automata.LoadState(state.AutomataState, state.AutomataOffset);
                var nextZeroState = automata.GetNextState(0);

                foreach (var letter in state.Node.Children.Keys)
                {
                    AutomatState? nextState;

                    if ((state.AutomataOffset < typo.Length && typo[state.AutomataOffset] == letter)
                        || (state.AutomataOffset < typo.Length - 1 && typo[state.AutomataOffset + 1] == letter)
                        || (state.AutomataOffset < typo.Length - Constants.MaxLevTLength && typo[state.AutomataOffset + Constants.MaxLevTLength] == letter))
                    {
                        nextState = automata.GetNextState(
                            automata.GetCharacteristicVector(letter, state.AutomataOffset));
                    }
                    else
                    {
                        nextState = nextZeroState;
                    }

                    if (nextState == null) continue;

                    var nextNode = state.Node.Children[letter];
                    if (nextNode.Children.Count > 0)
                    {
                        stack.Push(new FuzzyCorrectorState(nextNode, nextState.Value.State,
                            nextState.Value.Offset));
                    }

                    if ((nextNode.IsCharsArray || !includeOnlyCharacters) &&
                        automata.IsAcceptState(nextState.Value.State, nextState.Value.Offset))
                    {
                        corrections.Add(nextNode);
                    }
                }
            }

            return corrections;
        }

        private static int PrepareSubstrings(string typo, out string left, out string right, out string rleft,
            out string rright)
        {
            var rlen = typo.Length / 2;
            var llen = typo.Length - rlen;

            left = typo.Substring(0, llen);
            right = typo.Substring(llen);
            rleft = new string(left.Reverse().ToArray());
            rright = new string(right.Reverse().ToArray());

            return llen;
        }
    }
}