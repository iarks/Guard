using System.Collections.Generic;

namespace Guard
{
    public struct GuardClause<T>
    {
        private T _defaultValue;
        private readonly T _value;

        public string ParameterName { get; }

        private bool _isCurrentEvaluationSuccessSuccess;

        private readonly List<string> _errorMessages;

        public string Errors => string.Join('|', _errorMessages);
        
        public bool IsCurrentEvaluationSuccess
        {
            get => _isCurrentEvaluationSuccessSuccess;
            set
            {
                _isCurrentEvaluationSuccessSuccess = value;
                IsCompleteClauseEvaluationSuccess = IsCompleteClauseEvaluationSuccess && _isCurrentEvaluationSuccessSuccess;
            }
        }

        public T Value => IsCompleteClauseEvaluationSuccess ? _value : _defaultValue;

        public bool IsCompleteClauseEvaluationSuccess { get; private set; }

        public GuardClause(T value)
        {
            _value = value;
            ParameterName = string.Empty;
            _isCurrentEvaluationSuccessSuccess = true;
            IsCompleteClauseEvaluationSuccess = true;
            _defaultValue = default;
            _errorMessages = new List<string>();
        }
        
        public GuardClause(T value, string parameterName)
        {
            _value = value;
            ParameterName = parameterName;
            _isCurrentEvaluationSuccessSuccess = true;
            IsCompleteClauseEvaluationSuccess = true;
            _defaultValue = default;
            _errorMessages = new List<string>();
        }

        public GuardClause<T> WithDefaultValue(T defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public void AddErrorMessage(string message)
        {
            _errorMessages.Add(message);
        }
    }
}