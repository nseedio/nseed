using System;

namespace GettingThingsDone.Contracts.Model
{
    /// <summary>
    /// Represents a GTD action.
    /// </summary>
    public class Action : Entity
    {
        /// <summary>
        /// Gets or sets action title. Title is mandatory.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets optional due date.
        /// </summary>
        public DateTimeOffset? DueDate { get; set; }

        /// <summary>
        /// Gets or sets optional date on which the action is planned to be done.
        /// It must be smaller then <see cref="DueDate"/>.
        /// </summary>
        public DateTimeOffset? DoOn { get; set; }

        /// <summary>
        /// Gets or sets the date on which action is done, or null if the action is still undone.
        /// </summary>
        public DateTimeOffset? DoneAt { get; set; }

        /// <summary>
        /// Gets a value indicating whether true if the action is done. A done action cannot be changed.
        /// </summary>
        public bool IsDone => DoneAt != null;

        /// <summary>
        /// Gets or sets the ID of the list to which this action belongs or null if it doesn't belong to any list.
        /// </summary>
        public int? ListId { get; set; }

        /// <summary>
        /// Gets or sets the list to which this action belongs or null if it doesn't belong to any list.
        /// </summary>
        public ActionList List { get; set; }

        /// <summary>
        /// Gets or sets the ID of the project to which this action belongs or null if it doesn't belong to any project.
        /// </summary>
        public int? ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the project to which this action belongs or null if it doesn't belong to any project.
        /// </summary>
        public Project Project { get; set; }
    }
}
