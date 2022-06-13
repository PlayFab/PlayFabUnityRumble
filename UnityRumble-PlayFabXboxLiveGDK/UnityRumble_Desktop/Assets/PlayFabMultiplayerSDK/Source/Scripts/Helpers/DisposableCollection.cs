namespace PlayFab.Multiplayer
{
    using System;
    using System.Collections.Generic;

    public class DisposableCollection : IDisposable
    {
        private readonly List<IDisposable> disposables;

        public DisposableCollection()
        {
            this.disposables = new List<IDisposable>();
        }

        ~DisposableCollection()
        {
            this.Dispose(isDisposing: false);
        }

        public void Dispose()
        {
            this.Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        public T Add<T>(T disposable) where T : IDisposable
        {
            this.disposables.Add(disposable);
            return disposable;
        }

        private void Dispose(bool isDisposing)
        {
            foreach (DisposableBuffer disposable in this.disposables)
            {
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
