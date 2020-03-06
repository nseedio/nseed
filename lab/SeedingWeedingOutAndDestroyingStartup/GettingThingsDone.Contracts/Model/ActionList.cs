﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GettingThingsDone.Contracts.Model
{
    /// <summary>
    /// Represents a GTD action list.
    /// </summary>
    public class ActionList : Entity
    {
        public ActionList()
        {
            Actions = new Collection<Action>();
        }

        /// <summary>
        /// Gets or sets list name. Name is mandatory.
        /// </summary>
        public string Name { get; set; }

        public ICollection<Action> Actions { get; set; }
    }
}
