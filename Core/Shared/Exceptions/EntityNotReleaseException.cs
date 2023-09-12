﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Exceptions
{
    public class EntityNotReleaseException : Exception
    {
        public EntityNotReleaseException()
        {}

        public EntityNotReleaseException(string message)
            : base(message)
        {}


        public EntityNotReleaseException(string message, Exception inner)
            : base(message, inner)
        {}

        public EntityNotReleaseException(string entity, int id)
            : base("A '" + entity + "' with id{" + id + "} isn't release.")
        {}
    }
}
