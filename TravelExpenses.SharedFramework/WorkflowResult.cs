namespace TravelExpenses.SharedFramework
{
    using Newtonsoft.Json;
    using System;

    public class WorkflowResult
    {
        [JsonIgnore]
        private readonly string[] _errorMessages;
        [JsonIgnore]
        private readonly bool _success;

        /// <summary>
        /// Indicates a successful workflow
        /// </summary>
        public WorkflowResult() : this(Array.Empty<string>(), true)
        {

        }

        /// <summary>
        /// Indicates a unsuccessfull workflow with multiple error messages
        /// </summary>
        /// <param name="errorMessages"></param>
        public WorkflowResult(string[] errorMessages) : this(errorMessages, false)
        {
        }

        /// <summary>
        /// Indicates a unsuccessfull workflow with a single error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public WorkflowResult(string errorMessage) : this(new [] { errorMessage }, false)
        {
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="errorMessages"></param>
        private WorkflowResult(string[] errorMessages, bool success)
        {
            _errorMessages = errorMessages ?? throw new ArgumentNullException(nameof(errorMessages));
            _success = success;
        }

        /// <summary>
        /// Indicates success of the workflow
        /// </summary>
        [JsonIgnore]
        public bool Success => _success;

        /// <summary>
        /// Errors of the workflow
        /// </summary>
        [JsonProperty]
        public string[] Errors => _errorMessages;
    }
}
