using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackHenry.Services.TwitterService.Application.Exceptions
{
    /// <summary>
    /// Custom exception used when an entity is not found.
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        /// <param name="message">Not found message.</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

    }
}
