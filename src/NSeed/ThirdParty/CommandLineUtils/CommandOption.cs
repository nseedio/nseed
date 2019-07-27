// Copyright (c) Nate McMaster.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file has been modified from the original form. See Notice.txt in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils.Validation;

namespace McMaster.Extensions.CommandLineUtils
{
    /// <summary>
    /// Represents one or many command line option that is identified by flag proceeded by '-' or '--'.
    /// Options are not positional. Compare to <see cref="CommandArgument"/>.
    /// </summary>
    public class CommandOption
    {
        /// <summary>
        /// Initializes a new <see cref="CommandOption"/>.
        /// </summary>
        /// <param name="template">The template string. This is parsed into <see cref="ShortName"/> and <see cref="LongName"/>.</param>
        /// <param name="optionType">The option type.</param>
        public CommandOption(string template, CommandOptionType optionType)
        {
#pragma warning disable 618
            Template = template;
#pragma warning restore 618
            OptionType = optionType;

            var separators = (optionType == CommandOptionType.SingleOrNoValue)
                ? new[] { ' ', '|', ':', '=', '[', ']' }
                : new[] { ' ', '|', ':', '=' };
            foreach (var part in template.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                if (part.StartsWith("--"))
                {
                    LongName = part.Substring(2);
                }
                else if (part.StartsWith("-"))
                {
                    var optName = part.Substring(1);

                    // If there is only one char and it is not an English letter, it is a symbol option (e.g. "-?")
                    if (optName.Length == 1 && !IsEnglishLetter(optName[0]))
                    {
                        SymbolName = optName;
                    }
                    else
                    {
                        ShortName = optName;
                    }
                }
                else if (part.StartsWith("<") && part.EndsWith(">"))
                {
                    ValueName = part.Substring(1, part.Length - 2);
                }
                else
                {
                    throw new ArgumentException($"Invalid template pattern '{template}'", nameof(template));
                }
            }

            if (string.IsNullOrEmpty(LongName) && string.IsNullOrEmpty(ShortName) && string.IsNullOrEmpty(SymbolName))
            {
                throw new ArgumentException($"Invalid template pattern '{template}'", nameof(template));
            }
        }

        internal CommandOption(CommandOptionType type)
        {
            OptionType = type;
        }

        /// <summary>
        /// <para>
        /// This property is obsolete and will be removed in a future version.
        /// </para>
        /// <para>
        /// The argument template.
        /// </para>
        /// </summary>
        [Obsolete("This property is obsolete and will be removed in a future version.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string? Template { get; set; }

        /// <summary>
        /// The short command line flag used to identify this option. On command line, this is preceeded by a single '-{ShortName}'.
        /// </summary>
        public string? ShortName { get; set; }

        /// <summary>
        /// The long command line flag used to identify this option. On command line, this is preceeded by a double dash: '--{LongName}'.
        /// </summary>
        public string? LongName { get; set; }

        /// <summary>
        /// Can be used in addition to <see cref="ShortName"/> to add a single, non-English character.
        /// Example "-?".
        /// </summary>
        public string? SymbolName { get; set; }

        /// <summary>
        /// The name of value(s) shown in help text when <see cref="OptionType"/> is not <see cref="CommandOptionType.NoValue"/>.
        /// </summary>
        public string? ValueName { get; set; }

        /// <summary>
        /// A description of this option to show in generated help text.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Any values found during parsing, if any.
        /// </summary>
        public List<string?> Values { get; } = new List<string?>();

        /// <summary>
        /// Defines the type of the option.
        /// </summary>
        public CommandOptionType OptionType { get; private set; }

        /// <summary>
        /// Determines if this option should be shown in generated help text.
        /// </summary>
        public bool ShowInHelpText { get; set; } = true;

        /// <summary>
        /// Determines if subcommands added to <see cref="CommandLineApplication.Commands"/>
        /// should also have access to this option.
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        /// A collection of validators that execute before invoking <see cref="CommandLineApplication.OnExecute(Func{int})"/>.
        /// When validation fails, <see cref="CommandLineApplication.ValidationErrorHandler"/> is invoked.
        /// </summary>
        public ICollection<IOptionValidator> Validators { get; } = new List<IOptionValidator>();

        /// <summary>
        /// Attempt to parse the value that follows after the flag.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryParse(string? value)
        {
            switch (OptionType)
            {
                case CommandOptionType.MultipleValue:
                    Values.Add(value);
                    break;
                case CommandOptionType.SingleOrNoValue:
                case CommandOptionType.SingleValue:
                    if (Values.Any())
                    {
                        return false;
                    }
                    Values.Add(value);
                    break;
                case CommandOptionType.NoValue:
                    if (value != null)
                    {
                        return false;
                    }

                    // Add a value so .HasValue() == true
                    // Also, the count can be used to determine how many times a flag was specified
                    Values.Add(null);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return true;
        }

        /// <summary>
        /// True when <see cref="Values"/> is not empty.
        /// </summary>
        /// <returns></returns>
        public bool HasValue()
        {
            return Values.Any();
        }

        /// <summary>
        /// Returns the first element of <see cref="Values"/>, if any.
        /// </summary>
        /// <returns></returns>
        public string? Value()
        {
            return HasValue() ? Values[0] : null;
        }

        /// <summary>
        /// Generates the template string in the format "-{Symbol}|-{Short}|--{Long} &lt;{Value}&gt;" for display in help text.
        /// </summary>
        /// <returns>The template string</returns>
        internal string ToTemplateString()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SymbolName))
            {
                sb.Append('-').Append(SymbolName);
            }

            if (!string.IsNullOrEmpty(ShortName))
            {
                if (sb.Length > 0)
                {
                    sb.Append('|');
                }

                sb.Append('-').Append(ShortName);
            }

            if (!string.IsNullOrEmpty(LongName))
            {
                if (sb.Length > 0)
                {
                    sb.Append('|');
                }

                sb.Append("--").Append(LongName);
            }

            if (!string.IsNullOrEmpty(ValueName) && OptionType != CommandOptionType.NoValue)
            {
                if (OptionType == CommandOptionType.SingleOrNoValue)
                {
                    sb.Append("[:<").Append(ValueName).Append(">]");
                }
                else
                {
                    sb.Append(" <").Append(ValueName).Append('>');
                }
            }

            return sb.ToString();
        }


        private bool IsEnglishLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }
    }
}
