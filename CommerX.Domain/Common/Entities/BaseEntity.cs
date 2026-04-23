using System;

namespace CommerX.Domain.Common.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }
        public Guid CreatedBy { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public Guid ModifiedBy { get; protected set; }
        public DateTime? ModifiedOn { get; protected set; }

        protected BaseEntity(
        Guid id = default,
        Guid createdby = default,
        DateTime createdon = default)

        {
           Id = id == default ? Guid.NewGuid() : id;
           CreatedBy = createdby == default ? Guid.Empty : createdby;
           CreatedOn = createdon == default ? DateTime.UtcNow : createdon;

           ModifiedBy = Guid.Empty;
           ModifiedOn = null;
        }

        // 1. ¿Por qué el constructor de BaseEntity es protected y no private ?
        //2. ¿Por qué ModifiedOn es DateTime? (nullable) mientras que CreatedOn es DateTime(no
        //nullable)?
        //3. Si un Customer nunca fue modificado, ¿cuál es el valor de ModifiedOn ? ¿Cómo lo
        //verificarías en el código con un if ?
        //4. ¿Qué ventaja tiene usar parámetros con valor default en lugar de dos constructores
        //separados?
    }
}
