using System.Collections.Generic;

namespace NpgSQL.CRUDBuilder.Domain.IndexSearch.FuzzySearch.TrieDataStructure
{
    internal class Trie
    {
        public TrieNode root = new TrieNode(string.Empty);

        public Trie() { }

        public Trie(IEnumerable<string> lexicon)
        {
            foreach (var word in lexicon)
            {
                InsertCharacters(word);
            }
        }

        public void InsertCharacters(string characters)
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
        
        public bool СontainsPrefix(string prefix)
        {
            return Сontains(prefix, false);
        }
        
        public bool СontainsWord(string word)
        {
            return Сontains(word, true);
        }
        
        public TrieNode GetChars(string word)
        {
            var node = GetNode(word);
            return node != null && node.IsCharsArray ? node : null;
        }
        
        public TrieNode GetPrefix(string prefix)
        {
            return GetNode(prefix);
        }
        
        private bool Сontains(string argString, bool word)
        {
            var node = GetNode(argString);
            return (node != null && node.IsCharsArray && word) || (!word && node != null);
        }
        
        public TrieNode GetNode(string argString)
        {
            return root.GetChildNodes(argString);
        }
    }
}