using SimpleProject.Shared.Enums;

namespace SimpleProject.Shared.Misc
{
    public class CircuitBreaker
    {
        protected Exception? _lastException = null;

        protected readonly object _lock = new object();

        protected CircuitBreakerState _state = CircuitBreakerState.Closed;

        public async Task<T> Execute<T>(Func<Task<T>> func)
        {
            var monitorTryEnter = false;

            try
            {
                if (_state == CircuitBreakerState.Open)
                {
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
            _lastException = null;

            _state = CircuitBreakerState.Closed;
        }

        public void Trip(Exception exception)
        {
            _lastException = exception;

            _state = CircuitBreakerState.Open;
        }
    }
}
