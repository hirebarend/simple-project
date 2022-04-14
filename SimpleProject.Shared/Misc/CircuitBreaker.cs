using SimpleProject.Shared.Enums;

namespace SimpleProject.Shared.Misc
{
    public class CircuitBreaker
    {
        protected int _exceptionCount = 0;

        protected Exception? _lastException = null;

        protected DateTimeOffset? _lastExceptionDateTimeOffset = null;

        protected readonly object _lock = new object();

        protected CircuitBreakerState _state = CircuitBreakerState.Closed;

        protected int _totalCount = 0;

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            _totalCount += 1;

            var monitorTryEnter = false;

            try
            {
                if (_state == CircuitBreakerState.Open)
                {
                    if (_lastException != null && _lastExceptionDateTimeOffset != null && DateTimeOffset.UtcNow.Subtract(_lastExceptionDateTimeOffset.Value).TotalSeconds <= 5)
                    {
                        throw _lastException;
                    }

                    Monitor.TryEnter(_lock, ref monitorTryEnter);

                    if (monitorTryEnter)
                    {
                        HalfOpen();

                        var result = await func();

                        Reset();

                        return result;
                    }
                }

                return await func();
            }
            catch (Exception exception)
            {
                Trip(exception);

                throw;
            }
            finally
            {
                if (monitorTryEnter)
                {
                    Monitor.Exit(_lock);
                }
            }
        }

        public void HalfOpen()
        {
            _state = CircuitBreakerState.HalfOpen;
        }

        public void Reset()
        {
            _exceptionCount = 0;

            _lastException = null;

            _lastExceptionDateTimeOffset = null;

            _state = CircuitBreakerState.Closed;

            _totalCount = 0;
        }

        public void Trip(Exception exception)
        {
            _exceptionCount += 1;

            if (_state == CircuitBreakerState.Closed)
            {
                _lastException = exception;

                _lastExceptionDateTimeOffset = DateTimeOffset.UtcNow;

                var exceptionRate = Convert.ToDouble(_exceptionCount) / Convert.ToDouble(_totalCount);

                if (exceptionRate >= 0.25)
                {
                    _exceptionCount = 0;

                    _state = CircuitBreakerState.Open;

                    _totalCount = 0;
                }
            }
        }
    }
}
