﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GettingThingsDone.Contracts.Model
{
    /// <summary>
    /// Represents a GTD project.
    /// </summary>
    public class Project : Entity
    {
        public Project()
        {
            Actions = new Collection<Action>();
        }

        /// <summary>
        /// Gets or sets project name. Name is mandatory.
        /// </summary>
        public string Name { get; set; }

        public ICollection<Action> Actions { get; set; }
    }
}
