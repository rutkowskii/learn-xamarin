using System;
using System.Threading.Tasks;

namespace learn_xamarin.Utils
{
    public static class AsyncOp
    {
        public static AsyncOp<T> Get<T>(Func<Task<T>> asyncOp, Action<T> onSuccess, Action<Exception> onFailure, Action onCancel)
        {
            return new AsyncOp<T>(asyncOp, onSuccess, onFailure, onCancel);
        }
        
        public static AsyncOp<T> Get<T>(Func<Task<T>> asyncOp)
        {
            return new AsyncOp<T>(asyncOp, x => { }, e => { }, () => { });
        }
    }

    public class AsyncOp<T>
    {
        private readonly Func<Task<T>> _taskPrep;
        private readonly Action<T> _onSuccess;
        private readonly Action<Exception> _onFailure;
        private readonly Action _onCancel;

        public AsyncOp(Func<Task<T>> taskPrep, Action<T> onSuccess, Action<Exception> onFailure, Action onCancel)
        {
            _taskPrep = taskPrep;
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _onCancel = onCancel;
        }

        public async void Run()
        {
            var asyncOpResult = await RunCore();
            if (asyncOpResult.ResultFlag == OperationResult.Completed)
            {
                _onSuccess?.Invoke(asyncOpResult.Result);
            }
        }

        private async Task<AsyncOpResult<T>> RunCore()
        {
            try
            {
                var task = _taskPrep();
                var resultCore = await task;
                return new AsyncOpResult<T>
                {
                    ResultFlag = OperationResult.Completed, 
                    Result = resultCore
                };
            }
            catch (OperationCanceledException e)
            {
                _onCancel.Invoke();
                return new AsyncOpResult<T>{ResultFlag = OperationResult.Cancelled};
            }
            catch (Exception e)
            {
                _onFailure?.Invoke(e);
                return new AsyncOpResult<T>{ResultFlag = OperationResult.Failed};
            }
        }
        
        enum OperationResult{ Cancelled, Completed, Failed}

        class AsyncOpResult<T>
        {
            public OperationResult ResultFlag { get; set; }
            public T Result { get; set; }
        }
    }
}