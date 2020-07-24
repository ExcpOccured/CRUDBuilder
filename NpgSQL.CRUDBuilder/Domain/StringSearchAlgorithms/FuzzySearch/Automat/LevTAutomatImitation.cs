using System;
using NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms.FuzzySearch.Models;

namespace NpgSQL.CRUDBuilder.Domain.StringSearchAlgorithms.FuzzySearch.Automat
{
    internal class LevTAutomatImitation
    {
        internal readonly int AllowedModificationsCount;

        internal readonly int CharsLenght;

        internal readonly int VectorLength;

        internal readonly int VectorCount;

        internal readonly string Chars;

        internal readonly int StateCount;

        internal int CurrentState { get; private set; }

        internal int CurrentOffset { get; private set; }

        private sbyte[,] _stateTransitions;

        private sbyte[,] _offsetIncrements;

        private const string EmptyInputCharactersExceptionMessage = "Characters should have one or more letters";

        private const string EditDistanceIsOutOfRangeExceptionMessage = "Supported edit distances are 1 and 2";

        private const string OffsetLengthExceptionMessage = "Offset should be between 0 and ";

        internal LevTAutomatImitation(string chars, int allowedModificationsCount)
        {
            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentException(EmptyInputCharactersExceptionMessage);
            }

            if (allowedModificationsCount < 0 || allowedModificationsCount > 2)
            {
                throw new ArgumentException(EditDistanceIsOutOfRangeExceptionMessage);
            }

            CharsLenght = chars.Length;
            Chars = chars;
            AllowedModificationsCount = allowedModificationsCount;
            VectorLength = 2 * AllowedModificationsCount + 1;
            VectorCount = (int) Math.Pow(2, VectorLength);
            StateCount = allowedModificationsCount == 1 ? 6 : 42;

            UpdateMatrixes();
        }

        internal bool IsInEmptyState => CurrentState < 0 || CurrentOffset < 0;

        internal int GetCharacteristicVector(char letter, int position)
        {
            var vector = 0;
            for (var index = position; index < Math.Min(Chars.Length, position + VectorLength); index++)
            {
                if (Chars[index] == letter)
                {
                    vector |= 1 << (VectorLength - 1) - (index - position);
                }
            }

            return vector;
        }

        internal bool IsAcceptState(int? state, int? offset)
        {
            if (state < 0 || offset < 0)
            {
                return false;
            }

            var distanceToEnd = CharsLenght - offset;

            return distanceToEnd <= VectorLength - 1
                   && ParametricDescription.IsAcceptState[AllowedModificationsCount - 1][state
                                                                                         ?? throw new
                                                                                             NullReferenceException(),
                       (int) distanceToEnd];
        }

        internal bool IsInAcceptState => IsAcceptState(CurrentState, CurrentOffset);

        protected void UpdateMatrixes()
        {
            var distanceToEnd = CharsLenght - CurrentOffset;
            switch (distanceToEnd)
            {
                case 0:
                    _stateTransitions = ParametricDescription.StateTransitions0[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements0[AllowedModificationsCount - 1];
                    break;
                case 1:
                    _stateTransitions = ParametricDescription.StateTransitions1[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements1[AllowedModificationsCount - 1];
                    break;
                case 2:
                    _stateTransitions = ParametricDescription.StateTransitions2[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements2[AllowedModificationsCount - 1];
                    break;
                case 3:
                    _stateTransitions = ParametricDescription.StateTransitions3[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements3[AllowedModificationsCount - 1];
                    break;
                case 4:
                    _stateTransitions = ParametricDescription.StateTransitions4[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements4[AllowedModificationsCount - 1];
                    break;
                default:
                    _stateTransitions = ParametricDescription.StateTransitions5[AllowedModificationsCount - 1];
                    _offsetIncrements = ParametricDescription.OffsetIncrements5[AllowedModificationsCount - 1];
                    break;
            }
        }

        internal void LoadState(int? state, int offset)
        {
            if (offset > CharsLenght || offset < 0)
            {
                throw new ArgumentException(string.Concat(OffsetLengthExceptionMessage, CharsLenght));
            }

            if (state >= StateCount || state < 0)
            {
                throw new ArgumentException("State should be between 0 and 5");
            }

            CurrentState = state ?? throw new NullReferenceException();
            CurrentOffset = offset;

            UpdateMatrixes();
        }

        internal AutomatState? GetNextState(int vector)
        {
            if (vector >= VectorCount)
            {
                throw new ArgumentException("Vector should be between 0 and " + (VectorCount - 1));
            }

            if (IsInEmptyState)
            {
                return null;
            }

            int nextState = _stateTransitions[vector, CurrentState];

            if (nextState >= 0)
            {
                return new AutomatState
                {
                    State = nextState,
                    Offset = (CurrentOffset + _offsetIncrements[vector, CurrentState])
                };
            }

            return null;
        }

        internal void NextState(int vector)
        {
            var automatState = GetNextState(vector);

            if (automatState != null)
            {
                CurrentOffset = (int) automatState?.Offset;
                CurrentState = (int) automatState?.State;

                UpdateMatrixes();
            }
            else
            {
                CurrentState = -1;
                CurrentOffset = -1;
            }
        }

        internal void NextState(char letter)
        {
            if (IsInEmptyState)
            {
                return;
            }

            var vector = GetCharacteristicVector(letter, CurrentOffset);
            NextState(vector);
        }

        internal bool AcceptCharsEquivalence(string chars)
        {
            LoadState(0, 0);

            foreach (var letter in chars)
            {
                NextState(letter);
            }

            return IsInAcceptState;
        }
    }
}