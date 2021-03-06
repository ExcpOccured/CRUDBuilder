using System.Collections.Generic;

namespace NpgSQL.CRUDBuilder.SDK.SearchingAlgorithms.FuzzySearch.TrieDataStructure
{
    internal class Trie
    {
        internal TrieNode root = new TrieNode(string.Empty);

        internal Trie() { }

        internal Trie(IEnumerable<string> lexicon)
        {
            foreach (var word in lexicon)
            {
                InsertCharacters(word);
            }
        }

        internal void InsertCharacters(string characters)
        {
            var argChars = characters.ToCharArray();
            var currentNode = root;

            foreach (var symbol in argChars)
            {
                if (!currentNode.ContainsChildValue(symbol))
                {
                    currentNode.AddChild(symbol, new TrieNode(currentNode.Key + symbol));
                }

                currentNode = currentNode.GetChildNodes(symbol);
            }

            currentNode.IsCharsArray = true;
        }
        
        internal bool СontainsPrefix(string prefix)
        {
            return Сontains(prefix, false);
        }
        
        internal bool СontainsWord(string word)
        {
            return Сontains(word, true);
        }
        
        internal TrieNode GetChars(string word)
        {
            var node = GetNode(word);
            return node != null && node.IsCharsArray ? node : null;
        }
        
        internal TrieNode GetPrefix(string prefix)
        {
            return GetNode(prefix);
        }
        
        private bool Сontains(string argString, bool word)
        {
            var node = GetNode(argString);
            return (node != null && node.IsCharsArray && word) || (!word && node != null);
        }
        
        internal TrieNode GetNode(string argString)
        {
            return root.GetChildNodes(argString);
        }
    }
}