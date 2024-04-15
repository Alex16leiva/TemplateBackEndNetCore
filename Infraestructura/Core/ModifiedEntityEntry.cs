using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infraestructura.Core
{
    internal class ModifiedEntityEntry
    {
        public EntityEntry EntityEntry
        {
            get { return _entityEntry; }
        }

        private EntityEntry _entityEntry;

        public string State
        {
            get { return _state; }
        }
        private string _state;

        public ModifiedEntityEntry(EntityEntry entityEntry, string state)
        {
            _entityEntry = entityEntry;
            _state = state;
        }
    }
}
