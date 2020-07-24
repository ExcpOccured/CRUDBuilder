using System.Collections.Generic;

namespace NpgSQL.CRUDBuilder.SDK.SearchingAlgorithms.FuzzySearch.TrieDataStructure
{
    internal class TrieNode
    {
        internal readonly string Key;

        internal Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();

        internal TrieNode(string key)
        {
            Key = key;
        }
        
        public override string ToString()
        {
            return Key;
        }

        internal bool AddChild(char symbol, TrieNode child)
        {
            Children.Add(symbol, child);
            return true;
        }

        internal bool ContainsChildValue(char letter)
        {
            return Children.ContainsKey(letter);
        }

        internal TrieNode GetChildNodes(char letter)
        {
            return Children.ContainsKey(letter) ? Children[letter] : null;
        }

        internal bool IsCharsArray { get; set; }

        internal TrieNode GetChildNodes(string charsOrPrefix)
        {
            return string.IsNullOrEmpty(charsOrPrefix) ? null : GetChildNodes(charsOrPrefix.ToCharArray());
        }

        internal TrieNode GetTrieNode(string chars)
        {
            var node = GetChildNodes(chars);
            return node != null && node.IsCharsArray ? node : null;
        }

        private TrieNode GetChildNodes(IReadOnlyList<char> letters)
        {
            var currentNode = this;
            
            for (var index = 0; index < letters.Count && currentNode != null; index++)
            {
                currentNode = currentNode.GetChildNodes(letters[index]);
                if (currentNode == null)
                {
                    return null;
                }
            }

            return currentNode;
        }
    }
}