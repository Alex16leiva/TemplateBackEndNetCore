namespace Aplicacion.Core
{
    public class BaseDisposable : IDisposable
    {
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) 
        { 
            if (disposing)
            {
                //free other states (managed objects).
            }
            //free your own state (unmanaged objects).
            //Set large fields to null.
        }

        ~BaseDisposable()
        {
            Dispose(false);
        }
    }
}
