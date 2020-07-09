using System.Collections.Generic;

namespace NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms.FuzzySearch.TrieDataStructure
{
    internal class TrieNode
    {
        public readonly string Key;

        public Dictionary<char, TrieNode> Children = new Dictionary<char, TrieNode>();

        public TrieNode(string key)
        {
            Key = key;
        }

        public bool AddChild(char symbol, TrieNode child)
        {
            Children.Add(symbol, child);
            return true;
        }

        public bool ContainsChildValue(char letter)
        {
            return Children.ContainsKey(letter);
        }

        public TrieNode GetChildNodes(char letter)
        {
            return Children.ContainsKey(letter) ? Children[letter] : null;
        }

        public bool IsCharsArray { get; set; }

        public TrieNode GetChildNodes(string charsOrPrefix)
        {
            return string.IsNullOrEmpty(charsOrPrefix) ? null : GetChildNodes(charsOrPrefix.ToCharArray());
        }

        public TrieNode GetChildNodes(char[] letters)
        {
            var currentNode = this;
            
            for (var index = 0; index < letters.Length && currentNode != null; index++)
            {
                currentNode = currentNode.GetChildNodes(letters[index]);
                if (currentNode == null)
                {
                    return null;
                }
            }

            return currentNode;
        }

        public TrieNode GetTrieNode(string chars)
        {
            var node = GetChildNodes(chars);
            return node != null && node.IsCharsArray ? node : null;
        }

        public override string ToString()
        {
            return Key;
        }
    }
}