namespace Domain.Shared
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        protected Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            if (isSuccess && value is null)
            {
                throw new ArgumentNullException(nameof(value), "Value cannot be null for success results.");
            }

            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

        public static Result<TValue> Success(TValue value) => new(value, true, Error.None);

        public static new Result<TValue> Failure(Error error) => new(default, false, error);
    }
}