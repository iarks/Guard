using System;

namespace Guard
{
    public static class Guard
    {
        public static GuardClause<T> Against<T>(T parameter)
        {
            return new GuardClause<T>(parameter);
        }
        
        public static GuardClause<T> Against<T>(T parameter, string parameterName)
        {
            return new GuardClause<T>(parameter, parameterName);
        }
    }

    public static class GuardExtensions
    {
        public static GuardClause<T> ShouldNotBeNull<T>(this GuardClause<T> clause)
        {
            if (clause.Value != null) return clause;
            clause.IsCurrentEvaluationSuccess = false;
            clause.AddErrorMessage("Parameter is null");

            return clause;
        }
        
        public static GuardClause<string> ShouldNotBeEmpty(this GuardClause<string> clause)
        {
            if (!string.IsNullOrWhiteSpace(clause.Value)) return clause;
            clause.IsCurrentEvaluationSuccess = false;
            clause.AddErrorMessage("Parameter is Empty");

            return clause;
        }
        
        public static GuardClause<T> ThrowArgumentException<T>(this GuardClause<T> clause)
        {
            if (!clause.IsCompleteClauseEvaluationSuccess)
            {
                throw new ArgumentException(clause.Errors, clause.ParameterName);
            }
            
            return clause;
        }
        
        public static GuardClause<T> ThrowArgumentNullException<T>(this GuardClause<T> clause)
        {
            if (!clause.IsCompleteClauseEvaluationSuccess)
            {
                throw new ArgumentNullException(clause.ParameterName, clause.Errors);
            }
            
            return clause;
        }
        
        public static GuardClause<T> ThrowCustomException<T, TU>(this GuardClause<T> clause) where TU:Exception, new()
        {
            if (!clause.IsCompleteClauseEvaluationSuccess)
            {
                throw (Exception)Activator.CreateInstance(typeof(TU), clause.Errors);
            }
            
            return clause;
        }
        
        public static GuardClause<T> ThrowCustomException<T, TU>(this GuardClause<T> clause, string message) where TU:Exception
        {
            if (!clause.IsCompleteClauseEvaluationSuccess)
            {
                throw (Exception)Activator.CreateInstance(typeof(TU), message);
            }
            
            return clause;
        }
        
        public static GuardClause<T> ValidationFailsIf<T>(this GuardClause<T> clause, 
            Predicate<T> customPredicate, string message)
        {
            if (!customPredicate.Invoke(clause.Value)) return clause;
            clause.IsCurrentEvaluationSuccess = false;
            clause.AddErrorMessage(message);

            return clause;
        }
    }
}